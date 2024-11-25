using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.DatabaseEntities;
using Shared.Extensions;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class UploadWildcardCertificateCommandHandler : IRequestHandler<UploadWildcardCertificateCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiWebService fortiWebService;

        public UploadWildcardCertificateCommandHandler(DatabaseLogger logger, FortiWebService fortiWebService)
        {
            this.logger = logger;
            this.fortiWebService = fortiWebService;
        }

        public async Task<bool> Handle(UploadWildcardCertificateCommand command, CancellationToken cancellationToken)
        {
            var result = await this.UploadLocalCertificateAsync(command.SslWildcard);
            if (result)
            {
                command.SslWildcard.ChangedLocalCertificate = false;
                command.SslWildcard.HasLocalCertificate = true;
            }

            return true;
        }

        private async Task<bool> UploadLocalCertificateAsync(SslWildcard sslWildcard)
        {
            this.logger.Information(sslWildcard, $"Upload certificate for: {sslWildcard.Name}");

            var certificateName = sslWildcard.GetCertificateName();
            var domainName = sslWildcard.Name;

            var certificates = await this.fortiWebService.GetLocalCertificatesAsync();
            var exists = certificates.Results.Any(x => x.Name == certificateName);
            if (exists)
            {
                this.logger.Warning(sslWildcard, $"Certificate {certificateName} for {sslWildcard.Name} exists");

                return false;
            }

            this.logger.Information(sslWildcard, $"Uploading certificate: {certificateName}");

            var result = await this.fortiWebService.UploadLocalCertificateAsync(certificateName, sslWildcard.PfxBundle, domainName);
            this.logger.Debug(sslWildcard, $"UploadLocalCertificateAsync: {result.Json}");

            if (result.HasError)
            {
                this.logger.Information(sslWildcard, $"Upload certificate failed: {certificateName} with errorcode {result.Result.ErrorCode}");

                return false;
            }

            return true;
        }
    }
}