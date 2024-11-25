using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Cdn;
using Application.Infrastructure;
using Cre8ion.Common;
using Cre8ion.Database;
using Cre8ion.Features.Synchronization;
using MediatR;
using Shared;
using Shared.Extensions;
using Shared.Services;

namespace Application.Features
{
    public class CdnSynchronizer : ISynchronizer, IService
    {
        private readonly CommandLineOptions options;
        private readonly DatabaseLogger logger;
        private readonly IMediator mediator;
        private readonly DomainService domainService;

        public CdnSynchronizer(CommandLineOptions options, DatabaseLogger logger, IMediator mediator, DomainService domainService)
        {
            this.options = options;
            this.logger = logger;
            this.mediator = mediator;
            this.domainService = domainService;
        }

        public async Task<string> SynchronizeAsync(DateTime? lastSyncDate)
        {
            await this.ProcessBackendsAsync(false);
            await this.ProcessFrontendsAsync();
            await this.ProcessBackendsAsync(true);

            return "Done!";
        }

        public async Task<bool> ProcessBackendsAsync(bool processDeletes)
        {
            var cdnServer = this.options.Orangebag
                ? CdnServer.Orangebag
                : CdnServer.Default;

            var backends = await this.domainService.GetBackendsAsync(processDeletes, cdnServer);
            if (!backends.Any())
            {
                return true;
            }

            this.logger.Information($"Total backends: {backends.Count}");

            foreach (var backend in backends)
            {
                if (processDeletes)
                {
                    if (backend.IsDeleted)
                    {
                        backend.Status = EntityStatus.Archive;
                    }
                }
            }

            await this.domainService.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ProcessFrontendsAsync()
        {
            var cdnServer = this.options.Orangebag
                ? CdnServer.Orangebag
                : CdnServer.Default;

            var frontends = await this.domainService.GetFrontendsAsync(cdnServer);

            this.logger.Information($"Total frontends: {frontends.Count}");

            foreach (var frontend in frontends)
            {
                if (!frontend.IsValidDomain())
                {
                    this.logger.Information(frontend, $"Invalid domain: {frontend.Name}");

                    continue;
                }

                var subDomains = frontend.SubDomains
                    .Where(x => x.Status == EntityStatus.Published)
                    .ToList();

                if (frontend.IsDeleted)
                {
                    foreach (var subDomain in subDomains)
                    {
                        subDomain.Status = EntityStatus.Archive;
                    }

                    if (frontend.SslOrders.AnyOutLastOrDefault(x => x.Status == EntityStatus.Published, out var sslOrder))
                    {
                        await this.mediator.Send(new CdnBindingsCommand(frontend, sslOrder));
                        await this.mediator.Send(new UploadCdnCertificateCommand(frontend, sslOrder));
                    }

                    frontend.Status = EntityStatus.Archive;

                    continue;
                }

                foreach (var subDomain in subDomains)
                {
                    if (subDomain.IsDeleted)
                    {
                        continue;
                    }
                }

                if (frontend.UseLetsEncrypt)
                {
                    if (frontend.SslOrders.AnyOutLastOrDefault(x => x.Status == EntityStatus.Published && x.IsTested && x.IsFinished && !x.HasLocalCertificate, out var sslOrder4))
                    {
                        if (frontend.Backend.IsCdn)
                        {
                            await this.mediator.Send(new UploadCdnCertificateCommand(frontend, sslOrder4));
                        }
                    }

                    if (frontend.SslOrders.AnyOutLastOrDefault(x => x.Status == EntityStatus.Published && x.IsTested && x.IsFinished && x.HasLocalCertificate && !x.HasServerNameIndication, out var sslOrder5))
                    {
                        if (frontend.Backend.IsCdn)
                        {
                            await this.mediator.Send(new CdnBindingsCommand(frontend, sslOrder5));
                        }
                    }
                }

                if (!frontend.UseLetsEncrypt && !frontend.UseWildcard)
                {
                    if (frontend.SslOrders.AnyOutLastOrDefault(x => x.Status == EntityStatus.Published && x.IsTested && x.IsFinished && !x.HasLocalCertificate, out var sslOrder1))
                    {
                        if (frontend.Backend.IsCdn)
                        {
                            await this.mediator.Send(new UploadCdnCertificateCommand(frontend, sslOrder1));
                        }
                    }

                    if (frontend.SslOrders.AnyOutLastOrDefault(x => x.Status == EntityStatus.Published && x.IsTested && x.IsFinished && x.HasLocalCertificate && !x.HasServerNameIndication, out var sslOrder2))
                    {
                        if (frontend.Backend.IsCdn)
                        {
                            await this.mediator.Send(new CdnBindingsCommand(frontend, sslOrder2));
                        }
                    }
                }
            }

            await this.domainService.SaveChangesAsync();

            return true;
        }
    }
}