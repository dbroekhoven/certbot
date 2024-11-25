using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Extensions;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class ServerPolicyRuleCommandHandler : IRequestHandler<ServerPolicyRuleCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiWebService fortiWebService;

        public ServerPolicyRuleCommandHandler(DatabaseLogger logger, FortiWebService fortiWebService)
        {
            this.logger = logger;
            this.fortiWebService = fortiWebService;
        }

        public async Task<bool> Handle(ServerPolicyRuleCommand command, CancellationToken cancellationToken)
        {
            if (command.Backend.HasServerPolicyRule && !command.Backend.ChangedServerPolicyRule && !command.Backend.IsDeleted)
            {
                return true;
            }

            if (!command.Backend.HasServerPolicyRule && !command.Backend.ChangedServerPolicyRule && command.Backend.IsDeleted)
            {
                return true;
            }

            this.logger.Information(command.Backend, $"Server policy rule for: {command.Backend.Name}");

            if (command.Backend.IsDeleted)
            {
                return await this.DeleteAsync(command);
            }

            if (command.Backend.ChangedServerPolicyRule)
            {
                return await this.UpdateAsync(command);
            }

            return await this.CreateAsync(command);
        }

        public async Task<bool> CreateAsync(ServerPolicyRuleCommand command)
        {
            var serverPoolName = command.Backend.GetServerPoolName();
            var contentRoutingName = command.Backend.GetContentRoutingName();
            var protectionProfile = command.Backend.ProtectionProfile;
            var serverPolicyName = command.Backend.GetServerPolicyName();

            var serverPolicyRules = await this.fortiWebService.GetServerPolicyRulesAsync(serverPolicyName);
            if (serverPolicyRules.Results.Any(x => x.ContentRoutingPolicyName == contentRoutingName))
            {
                this.logger.Warning(command.Backend, $"CreateAsync: Server policy rule failed: {contentRoutingName} already exists on {serverPolicyName}");

                command.Backend.HasServerPolicyRule = true;

                return true;
            }

            var result = await this.fortiWebService.CreateServerPolicyRuleAsync(serverPolicyName, contentRoutingName, protectionProfile);
            this.logger.Debug(command.Backend, $"CreateServerPolicyRuleAsync: {result.Json}");

            if (result.HasError)
            {
                this.logger.Warning(command.Backend, $"CreateAsync: Server policy rule failed: {serverPolicyName}, {contentRoutingName}, Errorcode {result.Result.ErrorCode}");

                return false;
            }

            this.logger.Information(command.Backend, $"CreateAsync: Server policy rule created: {serverPolicyName}, {contentRoutingName}");

            command.Backend.HasServerPolicyRule = true;

            return true;
        }

        public async Task<bool> UpdateAsync(ServerPolicyRuleCommand command)
        {
            var serverPoolName = command.Backend.GetServerPoolName();
            var contentRoutingName = command.Backend.GetContentRoutingName();
            var protectionProfile = command.Backend.ProtectionProfile;
            var serverPolicyName = command.Backend.GetServerPolicyName();

            var serverPolicyRules = await this.fortiWebService.GetServerPolicyRulesAsync(serverPolicyName);
            if (!serverPolicyRules.Results.Any(x => x.ContentRoutingPolicyName == contentRoutingName))
            {
                this.logger.Warning(command.Backend, $"UpdateAsync: Server policy rule failed:{contentRoutingName} does not exists on {serverPolicyName}");

                return false;
            }

            var rule = serverPolicyRules.Results.Find(x => x.ContentRoutingPolicyName == contentRoutingName);
            var result = await this.fortiWebService.UpdateServerPolicyRuleAsync(serverPolicyName, contentRoutingName, protectionProfile, rule.Id);
            this.logger.Debug(command.Backend, $"UpdateServerPolicyRuleAsync: {result.Json}");

            if (result.HasError)
            {
                this.logger.Warning(command.Backend, $"UpdateAsync: Server policy rule failed: {serverPolicyName}, {contentRoutingName}, Errorcode {result.Result.ErrorCode}");

                return false;
            }

            this.logger.Information(command.Backend, $"UpdateAsync: Server policy rule updated: {serverPolicyName}, {contentRoutingName}");

            command.Backend.ChangedServerPolicyRule = false;

            return true;
        }

        public async Task<bool> DeleteAsync(ServerPolicyRuleCommand command)
        {
            var serverPoolName = command.Backend.GetServerPoolName();
            var contentRoutingName = command.Backend.GetContentRoutingName();
            var protectionProfile = command.Backend.ProtectionProfile;
            var serverPolicyName = command.Backend.GetServerPolicyName();

            var serverPolicyRules = await this.fortiWebService.GetServerPolicyRulesAsync(serverPolicyName);
            if (!serverPolicyRules.Results.Any(x => x.ContentRoutingPolicyName == contentRoutingName))
            {
                this.logger.Warning(command.Backend, $"DeleteAsync: Server policy rule failed: {contentRoutingName} does not exists on {serverPolicyName}");

                command.Backend.HasServerPolicyRule = false;

                return false;
            }

            var rule = serverPolicyRules.Results.Find(x => x.ContentRoutingPolicyName == contentRoutingName);
            var result = await this.fortiWebService.DeleteServerPolicyRuleAsync(serverPolicyName, rule.Id);
            this.logger.Debug(command.Backend, $"DeleteServerPolicyRuleAsync: {result.Json}");

            if (result.HasError)
            {
                this.logger.Warning(command.Backend, $"DeleteAsync: Server policy rule failed: {serverPolicyName}, {contentRoutingName}, Errorcode {result.Result.ErrorCode}");

                return false;
            }

            this.logger.Information(command.Backend, $"DeleteAsync: Server policy rule deleted: {serverPolicyName}, {contentRoutingName}");

            command.Backend.HasServerPolicyRule = false;

            return true;
        }
    }
}