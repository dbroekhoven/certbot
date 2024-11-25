using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.DatabaseEntities;
using Shared.Extensions;
using Shared.FortiWebModels;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class UploadCertificateCommandHandler : IRequestHandler<UploadCertificateCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiWebService fortiWebService;
        private readonly DomainService domainService;

        public UploadCertificateCommandHandler(DatabaseLogger logger, FortiWebService fortiWebService, DomainService domainService)
        {
            this.logger = logger;
            this.fortiWebService = fortiWebService;
            this.domainService = domainService;
        }

        public async Task<bool> Handle(UploadCertificateCommand command, CancellationToken cancellationToken)
        {
            if (command.SslOrder.HasLocalCertificate && !command.Frontend.IsDeleted)
            {
                return false;
            }

            if (command.Frontend.IsDeleted)
            {
                return await this.DeleteAsync(command);
            }

            return await this.UploadAsync(command);
        }

        private async Task<bool> UploadAsync(UploadCertificateCommand command)
        {
            this.logger.Information(command.SslOrder, $"Upload certificate for: {command.Frontend.Name}");

            var certificateName = command.SslOrder.GetCertificateName(command.Frontend);

            this.logger.Information(command.SslOrder, $"Uploading certificate: {certificateName}");

            var result = await this.UploadLocalCertificateAsync(command.Frontend, command.SslOrder, certificateName);
            if (result)
            {
                command.SslOrder.HasLocalCertificate = true;
            }

            return true;
        }

        private async Task<bool> DeleteAsync(UploadCertificateCommand command)
        {
            var certificateName = command.SslOrder.GetCertificateName(command.Frontend);

            this.logger.Information(command.SslOrder, $"Delete certificate: {certificateName}");

            var result = await this.fortiWebService.DeleteLocalCertificateAsync(certificateName);
            this.logger.Debug(command.SslOrder, $"DeleteLocalCertificateAsync: {result.Json}");

            if (result.HasError)
            {
                this.logger.Error(command.Frontend, $"Error deleting certificate {certificateName}, Errorcode {result.Result.ErrorCode}");

                return false;
            }

            return true;
        }

        private async Task<bool> UploadLocalCertificateAsync(Frontend frontend, SslOrder sslOrder, string certificateName)
        {
            var certificates = await this.fortiWebService.GetLocalCertificatesAsync();
            var exists = certificates.Results.Any(x => x.Name == certificateName);
            if (exists)
            {
                this.logger.Warning(sslOrder, $"Certificate {certificateName} for {frontend.Name} exists");

                return true;
            }

            this.logger.Information(sslOrder, $"Certificate {certificateName} for {frontend.Name} does not exists");

            var data = await this.domainService.GetOrderDataAsync(sslOrder.Id);

            FortiWebLocalCertificate upload = null;

            if (data.PfxBundle == null)
            {
                upload = await this.fortiWebService.UploadLocalCertificateAsync(certificateName, data.Certificate, data.PrivateKey);
            }
            else
            {
                upload = await this.fortiWebService.UploadLocalCertificateAsync(certificateName, data.PfxBundle, frontend.Name);
            }

            this.logger.Debug(sslOrder, $"UploadLocalCertificateAsync: {upload.Json}");

            if (upload.HasError)
            {
                this.logger.Error(sslOrder, $"Error uploading certificate {certificateName} for {frontend.Name}, Errorcode {upload.Result.ErrorCode}");

                return false;
            }

            this.logger.Information(sslOrder, $"Certificate {certificateName} uploaded for {frontend.Name}");

            return true;
        }
    }
}