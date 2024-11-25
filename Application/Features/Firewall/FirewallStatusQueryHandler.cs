using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Services;

namespace Application.Features.Firewall
{
    public class FirewallStatusQueryHandler : IRequestHandler<FirewallStatusQuery, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiGateService fortiGateService;

        public FirewallStatusQueryHandler(DatabaseLogger logger, FortiGateService fortiGateService)
        {
            this.logger = logger;
            this.fortiGateService = fortiGateService;
        }

        public async Task<bool> Handle(FirewallStatusQuery query, CancellationToken cancellationToken)
        {
            this.logger.Information("Check FortiGate status.");

            var status = await this.fortiGateService.GetStatusAsync();

            this.logger.Debug($"GetStatusAsync: {status.Json}");

            this.logger.Information($"Running current version {status.Version} build {status.Build}");

            return true;
        }
    }
}