using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.DatabaseEntities;
using Shared.Extensions;
using Shared.Services;

namespace Application.Features.Cdn
{
    public class UploadCdnCertificateCommandHandler : IRequestHandler<UploadCdnCertificateCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiWebService fortiWebService;
        private readonly DomainService domainService;

        public UploadCdnCertificateCommandHandler(DatabaseLogger logger, FortiWebService fortiWebService, DomainService domainService)
        {
            this.logger = logger;
            this.fortiWebService = fortiWebService;
            this.domainService = domainService;
        }

        public async Task<bool> Handle(UploadCdnCertificateCommand command, CancellationToken cancellationToken)
        {
            if (command.SslOrder.HasLocalCertificate && !command.Frontend.IsDeleted)
            {
                return false;
            }

            if (command.Frontend.IsDeleted)
            {
                //return await this.DeleteAsync(command);
            }

            var data = await this.domainService.GetOrderDataAsync(command.SslOrder.Id);

            var result = this.Upload(command, data);
            if (result)
            {
                command.SslOrder.HasLocalCertificate = true;
            }

            return true;
        }

        private bool Upload(UploadCdnCertificateCommand command, SslOrderData data)
        {
            this.logger.Information(command.SslOrder, $"Upload certificate for: {command.Frontend.Name}");

            var certificateName = command.SslOrder.GetCertificateName(command.Frontend);

            this.logger.Information(command.SslOrder, $"Uploading certificate: {certificateName}");

            using var certificate = data.PfxBundle == null
                ? X509Certificate2.CreateFromPem(data.Certificate, data.PrivateKey)
                : new X509Certificate2(data.PfxBundle, command.Frontend.Name, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

            using var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            certificate.FriendlyName = command.SslOrder.GetCertificateName(command.Frontend);

            store.Open(OpenFlags.ReadWrite);
            store.Add(certificate);

            return true;
        }
    }
}