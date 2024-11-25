using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class RemoveUnusedCertificatesCommandHandler : IRequestHandler<RemoveUnusedCertificatesCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiWebService fortiWebService;

        public RemoveUnusedCertificatesCommandHandler(DatabaseLogger logger, FortiWebService fortiWebService)
        {
            this.logger = logger;
            this.fortiWebService = fortiWebService;
        }

        public async Task<bool> Handle(RemoveUnusedCertificatesCommand command, CancellationToken cancellationToken)
        {
            var today = System.DateTime.Today;
            var expiration = System.DateTime.Today.AddDays(7);

            var certificates = await this.fortiWebService.GetLocalCertificatesAsync();

            var totalUnused = certificates.Results
                .Where(x => !x.Name.StartsWith("wildcard-"))
                .Where(x => x.QRef == 0)
                .ToList();

            var unusedAndAlmostExpired = certificates.Results
                .Where(x => !x.Name.StartsWith("wildcard-"))
                .Where(x => x.QRef == 0)
                .Where(x => x.ValidTo < expiration)
                .ToList();

            this.logger.Information($"Total unused certificates: {totalUnused.Count}");
            this.logger.Information($"Unused and almost expired certificates: {unusedAndAlmostExpired.Count}");

            if (!unusedAndAlmostExpired.Any())
            {
                return false;
            }

            foreach (var item in unusedAndAlmostExpired)
            {
                this.logger.Information($"Removed unused certificate: {item.Name}");

                var result = await this.fortiWebService.DeleteLocalCertificateAsync(item.Name);
                this.logger.Debug($"DeleteLocalCertificateAsync: {result.Json}");
            }

            return true;
        }
    }
}