using System;
using System.Linq;
using System.Threading.Tasks;
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
    public class ManualSynchronizer : ISynchronizer, IService
    {
        private readonly DatabaseLogger logger;
        private readonly IMediator mediator;
        private readonly DomainService domainService;

        public ManualSynchronizer(DatabaseLogger logger, IMediator mediator, DomainService domainService)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.domainService = domainService;
        }

        public async Task<string> SynchronizeAsync(DateTime? lastSyncDate)
        {
            Console.Write("Enter domainname to renew: ");

            var domainName = Console.ReadLine();

            Console.Write("Create new order? Y/n: ");

            var newOrder = Console.ReadLine() != "n";

            await this.ProcessBackendsAsync(domainName);
            await this.ProcessFrontendsAsync(domainName, newOrder);

            return "Done!";
        }

        public async Task<bool> ProcessBackendsAsync(string domainName)
        {
            this.logger.Information($"Get backend '{domainName}' from database.");

            var backends = await this.domainService.GetBackendsAsync(domainName);
            if (!backends.Any())
            {
                return true;
            }

            foreach (var backend in backends)
            {
                if (!backend.IsValidDomain())
                {
                    this.logger.Information(backend, $"Invalid domain: {backend.Name}");

                    continue;
                }

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

            this.logger.Information($"Save changes.");

            await this.domainService.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ProcessFrontendsAsync(string domainName, bool newOrder)
        {
            this.logger.Information($"Get frontend '{domainName}' from database.");

            var frontends = await this.domainService.GetFrontendsAsync(domainName);

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
                    if (newOrder)
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
    }
}