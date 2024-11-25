using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class ProxyStatusQueryHandler : IRequestHandler<ProxyStatusQuery, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiWebService fortiWebService;

        public ProxyStatusQueryHandler(DatabaseLogger logger, FortiWebService fortiWebService)
        {
            this.logger = logger;
            this.fortiWebService = fortiWebService;
        }

        public async Task<bool> Handle(ProxyStatusQuery query, CancellationToken cancellationToken)
        {
            this.logger.Information("Check FortiWeb status.");

            var status = await this.fortiWebService.GetStatusAsync();
            this.logger.Debug($"GetStatusAsync: {status.Json}");

            this.logger.Information($"{status.Result.OperationMode} {status.Result.FirmwareVersion}, {status.Result.HaStatus} {status.Result.OperationMode}");

            return true;
        }
    }
}