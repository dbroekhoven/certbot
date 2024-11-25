using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Extensions;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class UrlRewriteCommandHandler : IRequestHandler<UrlRewriteCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiWebService fortiWebService;

        public UrlRewriteCommandHandler(DatabaseLogger logger, FortiWebService fortiWebService)
        {
            this.logger = logger;
            this.fortiWebService = fortiWebService;
        }

        public async Task<bool> Handle(UrlRewriteCommand command, CancellationToken cancellationToken)
        {
            if (command.SubDomain.HasRewriteRule && !command.SubDomain.ChangedRewriteRule && !command.SubDomain.IsDeleted)
            {
                return true;
            }

            this.logger.Information(command.SubDomain, $"Url rewrite for: {command.SubDomain.Name}");

            if (command.SubDomain.IsDeleted)
            {
                return await this.DeleteAsync(command);
            }

            if (command.SubDomain.ChangedRewriteRule)
            {
                return await this.UpdateAsync(command);
            }

            return await this.CreateAsync(command);
        }

        public async Task<bool> CreateAsync(UrlRewriteCommand command)
        {
            var rewriteRuleName = command.SubDomain.GetUrlRewriteRuleName();
            var rewriteRules = await this.fortiWebService.GetRewritingRulesAsync();
            if (rewriteRules.Results.Any(x => x.Name == rewriteRuleName))
            {
                this.logger.Error(command.SubDomain, $"Url rewrite failed: {command.SubDomain.Name} already exists in {rewriteRuleName}");

                command.SubDomain.HasRewriteRule = true;

                return false;
            }

            var create = await this.fortiWebService.CreateRewritingRuleAsync(rewriteRuleName, command.SubDomain.Frontend.Backend.RedirectUrl);
            this.logger.Debug(command.SubDomain, $"CreateRewritingRuleAsync: {create.Json}");
            this.logger.Information(command.SubDomain, $"Url rewrite created:  {command.SubDomain.Name} in {rewriteRuleName}");

            var createCondition = await this.fortiWebService.CreateRewritingRuleConditionAsync(rewriteRuleName, command.SubDomain.Name);
            this.logger.Debug(command.SubDomain, $"CreateRewritingRuleConditionAsync: {createCondition.Json}");
            this.logger.Information(command.SubDomain, $"Url rewrite condition created:  {command.SubDomain.Name} in {rewriteRuleName}");

            var updatePolicy = await this.fortiWebService.UpdateRewritingPolicyAsync(rewriteRuleName);
            this.logger.Debug(command.SubDomain, $"UpdateRewritingPolicyAsync: {createCondition.Json}");
            this.logger.Information(command.SubDomain, $"Url rewrite policy updated:  {command.SubDomain.Name} in {rewriteRuleName}");

            command.SubDomain.HasRewriteRule = true;

            return true;
        }

        public async Task<bool> DeleteAsync(UrlRewriteCommand command)
        {
            var rewriteRuleName = command.SubDomain.GetUrlRewriteRuleName();
            var policyRules = await this.fortiWebService.GetRewritePolicyRulesAsync();
            var policyRule = policyRules.Results.Find(x => x.Name == rewriteRuleName);
            if (policyRule == null)
            {
                this.logger.Error(command.SubDomain, $"Url rewrite failed: {command.SubDomain.Name} does not exists in {rewriteRuleName}");

                command.SubDomain.Status = Cre8ion.Database.EntityStatus.Archive;

                return false;
            }

            var updatePolicy = await this.fortiWebService.DeleteFromRewritingPolicyAsync(policyRule.Id);
            this.logger.Debug(command.SubDomain, $"DeleteFromRewritingPolicyAsync: {updatePolicy.Json}");
            this.logger.Information(command.SubDomain, $"Url rewrite policy updated:  {command.SubDomain.Name} in {rewriteRuleName}");

            var delete = await this.fortiWebService.DeleteRewritingRuleAsync(rewriteRuleName);
            this.logger.Debug(command.SubDomain, $"DeleteRewritingRuleAsync: {delete.Json}");
            this.logger.Information(command.SubDomain, $"Url rewrite deleted:  {command.SubDomain.Name} in {rewriteRuleName}");

            command.SubDomain.Status = Cre8ion.Database.EntityStatus.Archive;

            return true;
        }

        public async Task<bool> UpdateAsync(UrlRewriteCommand command)
        {
            var rewriteRuleName = command.SubDomain.GetUrlRewriteRuleName();

            var update = await this.fortiWebService.UpdateRewritingRuleAsync(rewriteRuleName, command.SubDomain.Frontend.Backend.RedirectUrl);
            this.logger.Debug(command.SubDomain, $"UpdateRewritingRuleAsync: {update.Json}");
            this.logger.Information(command.SubDomain, $"Url rewrite updated:  {command.SubDomain.Name} in {rewriteRuleName}");

            command.SubDomain.ChangedRewriteRule = false;

            return true;
        }
    }
}