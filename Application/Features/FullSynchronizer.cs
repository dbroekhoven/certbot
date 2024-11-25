using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Account;
using Application.Features.LetsEncrypt;
using Application.Features.Proxy;
using Cre8ion.Common;
using Cre8ion.Database;
using Cre8ion.Features.Synchronization;
using MediatR;
using Shared.Extensions;
using Shared.Services;

namespace Application.Features
{
    public class FullSynchronizer : ISynchronizer, IService
    {
        private readonly DatabaseLogger logger;
        private readonly IMediator mediator;
        private readonly DomainService domainService;

        public FullSynchronizer(DatabaseLogger logger, IMediator mediator, DomainService domainService)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.domainService = domainService;
        }

        public async Task<string> SynchronizeAsync(DateTime? lastSyncDate)
        {
            await this.ProcessWildcardsAsync();
            await this.ProcessBackendsAsync(false);
            await this.ProcessFrontendsAsync();
            await this.ProcessBackendsAsync(true);
            await this.ProcessOrderCleanupAsync();

            return "Done!";
        }

        public async Task<bool> ProcessStatusAsync()
        {
            var accountStatus = await this.mediator.Send(new AccountQuery());
            if (!accountStatus)
            {
                return false;
            }

            var proxyStatus = await this.mediator.Send(new ProxyStatusQuery());
            if (!proxyStatus)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ProcessWildcardsAsync()
        {
            this.logger.Information($"Get wildcards from database.");

            var wildcards = await this.domainService.GetWildcardsAsync();

            this.logger.Information($"Total wildcards: {wildcards.Count}");

            foreach (var wildcard in wildcards)
            {
                if (!wildcard.IsValidDomain())
                {
                    this.logger.Information(wildcard, $"Invalid domain: {wildcard.Name}");

                    continue;
                }

                if (!wildcard.HasLocalCertificate || wildcard.ChangedLocalCertificate)
                {
                    await this.mediator.Send(new UploadWildcardCertificateCommand(wildcard));
                }
            }

            this.logger.Information($"Save changes.");

            await this.domainService.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ProcessBackendsAsync(bool processDeletes)
        {
            this.logger.Information($"Get backends from database.");

            var backends = await this.domainService.GetBackendsAsync(processDeletes, Shared.CdnServer.None);
            if (!backends.Any())
            {
                return true;
            }

            this.logger.Information($"Total backends: {backends.Count}");

            foreach (var backend in backends)
            {
                if (!backend.IsValidDomain())
                {
                    this.logger.Information(backend, $"Invalid domain: {backend.Name}");

                    continue;
                }

                if (processDeletes)
                {
                    if (backend.IsDeleted)
                    {
                        this.logger.Information($"Delete backend: {backend.Name}");

                        var result1 = await this.mediator.Send(new ServerPolicyRuleCommand(backend));
                        var result2 = await this.mediator.Send(new ContentRoutingCommand(backend));
                        var result3 = await this.mediator.Send(new ServerPoolCommand(backend));

                        if (result1 && result2 && result3)
                        {
                            backend.Status = EntityStatus.Archive;
                        }

                        continue;
                    }
                }
                else
                {
                    if (!backend.IsCdn)
                    {
                        if (!backend.HasServerPool)
                        {
                            await this.mediator.Send(new ServerPoolCommand(backend));
                        }

                        if (!backend.HasServerPoolRule || backend.ChangedServerPoolRule)
                        {
                            await this.mediator.Send(new ServerPoolRuleCommand(backend));
                        }

                        if (!backend.HasContentRouting)
                        {
                            await this.mediator.Send(new ContentRoutingCommand(backend));
                        }

                        if ((backend.UseIPv6 && !backend.HasIPv6) || (!backend.UseIPv6 && backend.HasIPv6))
                        {
                            await this.mediator.Send(new VirtualAddressGroupCommand(backend));
                        }

                        if (!backend.HasServerPolicyRule || backend.ChangedServerPolicyRule)
                        {
                            await this.mediator.Send(new ServerPolicyRuleCommand(backend));
                        }

                        if (backend.ChangedServerPool)
                        {
                            await this.mediator.Send(new ContentRoutingCommand(backend));
                            await this.mediator.Send(new ServerPoolCommand(backend));
                        }
                    }
                }
            }

            this.logger.Information($"Save changes.");

            await this.domainService.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ProcessFrontendsAsync()
        {
            this.logger.Information($"Get frontends from database.");

            var frontends = await this.domainService.GetFrontendsAsync();

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
                    this.logger.Information($"Delete frontend: {frontend.Name}");

                    foreach (var subDomain in subDomains)
                    {
                        subDomain.Status = EntityStatus.Archive;
                    }

                    if (frontend.SslOrders.AnyOutLastOrDefault(x => x.Status == EntityStatus.Published, out var sslOrder))
                    {
                        if (!frontend.Backend.IsCdn)
                        {
                            await this.mediator.Send(new ServerNameIndicationCommand(frontend, sslOrder));
                            await this.mediator.Send(new UploadCertificateCommand(frontend, sslOrder));
                        }
                    }

                    if (frontend.HasWildcard)
                    {
                        await this.mediator.Send(new WildcardServerNameIndicationCommand(frontend));
                    }

                    frontend.Status = EntityStatus.Archive;

                    foreach (var subDomain in subDomains)
                    {
                        if (subDomain.IsDeleted)
                        {
                            if (subDomain.HasContentRoutingRule)
                            {
                                await this.mediator.Send(new ContentRoutingRuleCommand(subDomain));
                            }

                            if (frontend.Backend.IsRedirect && subDomain.HasRewriteRule)
                            {
                                await this.mediator.Send(new UrlRewriteCommand(subDomain));
                            }

                            if (frontend.SslOrders.AnyOutLastOrDefault(x => x.Status == EntityStatus.Published, out var sslOrder2))
                            {
                                await this.mediator.Send(new ServerNameIndicationCommand(frontend, sslOrder2, subDomain));
                            }
                        }
                    }

                    continue;
                }

                foreach (var subDomain in subDomains)
                {
                    if (!frontend.Backend.IsCdn && !subDomain.HasContentRoutingRule)
                    {
                        await this.mediator.Send(new ContentRoutingRuleCommand(subDomain));
                    }

                    if (!frontend.Backend.IsCdn && frontend.Backend.IsRedirect && (!subDomain.HasRewriteRule || subDomain.ChangedRewriteRule))
                    {
                        await this.mediator.Send(new UrlRewriteCommand(subDomain));
                    }
                }

                if (frontend.UseLetsEncrypt)
                {
                    var newOrTested = !frontend.SslOrders.Any();
                    if (newOrTested)
                    {
                        await this.mediator.Send(new NewOrdersCommand(frontend));
                    }

                    var almostExpired = frontend.SslOrders.Any(y => y.Status == EntityStatus.Published) && frontend.SslOrders.Where(y => y.Status == EntityStatus.Published).Max(y => y.NotAfter.Value).AddDays(-30) < DateTime.Today;
                    if (almostExpired || frontend.Reissue)
                    {
                        await this.mediator.Send(new NewOrdersCommand(frontend));
                    }

                    if (frontend.SslOrders.AnyOutLastOrDefault(x => x.Status == EntityStatus.Approval && !x.IsTested, out var sslOrder1))
                    {
                        await this.mediator.Send(new TestOrdersCommand(sslOrder1));
                    }

                    if (frontend.SslOrders.AnyOutLastOrDefault(x => x.Status == EntityStatus.Approval && x.IsTested && !x.IsValidated, out var sslOrder2))
                    {
                        await this.mediator.Send(new ValidateOrdersCommand(sslOrder2));
                    }

                    if (frontend.SslOrders.AnyOutLastOrDefault(x => x.Status == EntityStatus.Approval && x.IsTested && x.IsValidated && !x.IsFinished, out var sslOrder3))
                    {
                        await this.mediator.Send(new FinishOrdersCommand(sslOrder3));
                    }

                    if (frontend.SslOrders.AnyOutLastOrDefault(x => x.Status == EntityStatus.Published && x.IsTested && x.IsFinished && !x.HasLocalCertificate, out var sslOrder4))
                    {
                        if (!frontend.Backend.IsCdn)
                        {
                            await this.mediator.Send(new UploadCertificateCommand(frontend, sslOrder4));
                        }
                    }

                    if (frontend.SslOrders.AnyOutLastOrDefault(x => x.Status == EntityStatus.Published && x.IsTested && x.IsFinished && x.HasLocalCertificate && !x.HasServerNameIndication, out var sslOrder5))
                    {
                        if (!frontend.Backend.IsCdn)
                        {
                            await this.mediator.Send(new ServerNameIndicationCommand(frontend, sslOrder5));
                        }
                    }
                }

                if (!frontend.UseLetsEncrypt && !frontend.UseWildcard)
                {
                    if (frontend.SslOrders.AnyOutLastOrDefault(x => x.Status == EntityStatus.Published && x.IsTested && x.IsFinished && !x.HasLocalCertificate, out var sslOrder1))
                    {
                        if (!frontend.Backend.IsCdn)
                        {
                            await this.mediator.Send(new UploadCertificateCommand(frontend, sslOrder1));
                        }
                    }

                    if (frontend.SslOrders.AnyOutLastOrDefault(x => x.Status == EntityStatus.Published && x.IsTested && x.IsFinished && x.HasLocalCertificate && !x.HasServerNameIndication, out var sslOrder2))
                    {
                        if (!frontend.Backend.IsCdn)
                        {
                            await this.mediator.Send(new ServerNameIndicationCommand(frontend, sslOrder2));
                        }
                    }
                }

                if (frontend.UseWildcard && (!frontend.HasWildcard || frontend.ChangedWildcard))
                {
                    if (!frontend.Backend.IsCdn)
                    {
                        await this.mediator.Send(new WildcardServerNameIndicationCommand(frontend));
                    }
                }
            }

            this.logger.Information($"Save changes.");

            await this.domainService.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ProcessOrderCleanupAsync()
        {
            await this.mediator.Send(new ArchiveOldSslOrdersCommand());

            return true;
        }
    }
}