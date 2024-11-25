using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Web.Administration;
using Shared.DatabaseEntities;
using Shared.Services;

namespace Application.Features.Cdn
{
    public class CdnBindingsCommandHandler : IRequestHandler<CdnBindingsCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiWebService fortiWebService;
        private readonly DomainService domainService;

        public CdnBindingsCommandHandler(DatabaseLogger logger, FortiWebService fortiWebService, DomainService domainService)
        {
            this.logger = logger;
            this.fortiWebService = fortiWebService;
            this.domainService = domainService;
        }

        public async Task<bool> Handle(CdnBindingsCommand command, CancellationToken cancellationToken)
        {
            if (command.SslOrder.HasServerNameIndication && !command.Frontend.IsDeleted)
            {
                return false;
            }

            if (command.Frontend.IsDeleted)
            {
                //return await this.DeleteAsync(command);
            }

            var data = await this.domainService.GetOrderDataAsync(command.SslOrder.Id);

            var result = this.Bindings(command, data);
            if (result)
            {
                command.SslOrder.HasServerNameIndication = true;
            }

            return true;
        }

        private bool Bindings(CdnBindingsCommand command, SslOrderData data)
        {
            this.logger.Information(command.SslOrder, $"Set bindings for: {command.Frontend.Backend.Name}");

            using var certificate = data.PfxBundle == null
                ? X509Certificate2.CreateFromPem(data.Certificate, data.PrivateKey)
                : new X509Certificate2(data.PfxBundle, command.Frontend.Name, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

            using var manager = new ServerManager();

            var site = manager.Sites[command.Frontend.Backend.Name];
            var update = false;

            foreach (var binding in site.Bindings)
            {
                if (binding.Host == command.Frontend.Name)
                {
                    binding.CertificateHash = certificate.GetCertHash();
                    binding.CertificateStoreName = "My";

                    update = true;
                }
            }

            if (!update)
            {
                site.Bindings.Add($"*:443:{command.Frontend.Name}", certificate.GetCertHash(), "My", SslFlags.Sni);
            }

            manager.CommitChanges();

            return true;
        }
    }
}