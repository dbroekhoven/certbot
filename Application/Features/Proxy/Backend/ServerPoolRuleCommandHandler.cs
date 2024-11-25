using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cre8ion.Common.Extensions;
using MediatR;
using Shared.Extensions;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class ServerPoolRuleCommandHandler : IRequestHandler<ServerPoolRuleCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiWebService fortiWebService;

        public ServerPoolRuleCommandHandler(DatabaseLogger logger, FortiWebService fortiWebService)
        {
            this.logger = logger;
            this.fortiWebService = fortiWebService;
        }

        public async Task<bool> Handle(ServerPoolRuleCommand command, CancellationToken cancellationToken)
        {
            if (command.Backend.HasServerPoolRule && !command.Backend.ChangedServerPoolRule)
            {
                return true;
            }

            var serverPoolName = command.Backend.GetServerPoolName();

            this.logger.Information(command.Backend, $"Server pool rule {serverPoolName}");

            return await this.CreateAsync(command);
        }

        public async Task<bool> CreateAsync(ServerPoolRuleCommand command)
        {
            var serverPoolName = command.Backend.GetServerPoolName();
            var loadBalanced = command.Backend.IsLoadBalanced();
            var internalAddresses = command.Backend.InternalAddress.ToStringArray();

            var allServerPools = await this.fortiWebService.GetServerPoolsAsync();
            if (!allServerPools.Results.Any(x => x.Name == serverPoolName))
            {
                this.logger.Warning(command.Backend, $"Server pool {serverPoolName} does not exists");

                return false;
            }

            foreach (var internalAddress in internalAddresses)
            {
                var serverPoolRules = await this.fortiWebService.GetServerPoolRulesAsync(serverPoolName);
                if (serverPoolRules.Results.Any(x => x.Ip == internalAddress))
                {
                    this.logger.Error(command.Backend, $"Server pool rule failed: {internalAddress} already exists on {serverPoolName}");

                    command.Backend.HasServerPoolRule = true;

                    continue;
                }

                var result = await this.fortiWebService.CreateServerPoolRuleAsync(serverPoolName, internalAddress);
                this.logger.Debug(command.Backend, $"CreateServerPoolRuleAsync: {result.Json}");

                if (result.HasError)
                {
                    this.logger.Error(command.Backend, $"Server pool rule failed: {serverPoolName}, Address {internalAddress}, Errorcode {result.Result.ErrorCode}");

                    continue;
                }

                this.logger.Information(command.Backend, $"Server pool rule created: {serverPoolName}, Address {internalAddress}");
            }

            command.Backend.HasServerPoolRule = true;
            command.Backend.ChangedServerPoolRule = false;

            return true;
        }
    }
}