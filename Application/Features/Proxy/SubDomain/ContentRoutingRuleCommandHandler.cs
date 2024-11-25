using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Extensions;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class ContentRoutingRuleCommandHandler : IRequestHandler<ContentRoutingRuleCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiWebService fortiWebService;

        public ContentRoutingRuleCommandHandler(DatabaseLogger logger, FortiWebService fortiWebService)
        {
            this.logger = logger;
            this.fortiWebService = fortiWebService;
        }

        public async Task<bool> Handle(ContentRoutingRuleCommand command, CancellationToken cancellationToken)
        {
            if (command.SubDomain.HasContentRoutingRule && !command.SubDomain.IsDeleted)
            {
                return true;
            }

            this.logger.Information(command.SubDomain, $"Content routing rule for: {command.SubDomain.Name}");


            if (command.SubDomain.IsDeleted)
            {
                return await this.DeleteAsync(command);
            }

            return await this.CreateAsync(command);
        }

        public async Task<bool> CreateAsync(ContentRoutingRuleCommand command)
        {
            var contentRoutingName = command.SubDomain.Frontend.Backend.GetContentRoutingName();
            var contentRoutingRules = await this.fortiWebService.GetContentRoutingRulesAsync(contentRoutingName);
            if (contentRoutingRules.Results.Any(x => x.MatchExpression == command.SubDomain.Name))
            {
                this.logger.Error(command.SubDomain, $"Content routing rule failed: {command.SubDomain.Name} already exists in {contentRoutingName}");

                command.SubDomain.HasContentRoutingRule = true;

                return false;
            }

            var create = await this.fortiWebService.CreateContentRoutingRuleAsync(contentRoutingName, command.SubDomain.Name);
            this.logger.Debug(command.SubDomain, $"CreateContentRoutingRuleAsync: {create.Json}");

            if (create.HasError)
            {
                this.logger.Error(command.SubDomain, $"Content routing rule failed: {command.SubDomain.Name} in {contentRoutingName}, Errorcode {create.Result.ErrorCode}");

                return false;
            }

            this.logger.Information(command.SubDomain, $"Content routing rule created:  {command.SubDomain.Name} in {contentRoutingName}");

            command.SubDomain.HasContentRoutingRule = true;

            return true;
        }

        public async Task<bool> DeleteAsync(ContentRoutingRuleCommand command)
        {
            var contentRoutingName = command.SubDomain.Frontend.Backend.GetContentRoutingName();
            var contentRoutingRules = await this.fortiWebService.GetContentRoutingRulesAsync(contentRoutingName);
            if (!contentRoutingRules.Results.Any(x => x.MatchExpression == command.SubDomain.Name))
            {
                this.logger.Warning(command.SubDomain, $"Content routing rule failed: {command.SubDomain.Name} does not exists in {contentRoutingName}");

                command.SubDomain.Status = Cre8ion.Database.EntityStatus.Archive;

                return true;
            }

            var rule = contentRoutingRules.Results.Find(x => x.MatchExpression == command.SubDomain.Name);
            var delete = await this.fortiWebService.DeleteContentRoutingRuleAsync(contentRoutingName, rule.Id);
            this.logger.Debug(command.SubDomain, $"DeleteContentRoutingRuleAsync: {delete.Json}");

            if (delete.HasError)
            {
                this.logger.Error(command.SubDomain, $"Content routing rule failed: {command.SubDomain.Name} in {contentRoutingName}, Errorcode {delete.Result.ErrorCode}");

                return false;
            }

            this.logger.Information(command.SubDomain, $"Content routing rule deleted:  {command.SubDomain.Name} in {contentRoutingName}");

            command.SubDomain.Status = Cre8ion.Database.EntityStatus.Archive;

            return true;
        }
    }
}