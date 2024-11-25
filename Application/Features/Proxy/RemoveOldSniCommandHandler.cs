using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cre8ion.Common.Extensions;
using Cre8ion.Database;
using Cre8ion.Database.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.DatabaseEntities;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class RemoveOldSniCommandHandler : IRequestHandler<RemoveOldSniCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly DatabaseContext dbContext;
        private readonly FortiWebService fortiWebService;
        private readonly DomainService domainService;

        public RemoveOldSniCommandHandler(DatabaseLogger logger, DatabaseContext dbContext, FortiWebService fortiWebService, DomainService domainService)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.fortiWebService = fortiWebService;
            this.domainService = domainService;
        }

        public async Task<bool> Handle(RemoveOldSniCommand command, CancellationToken cancellationToken)
        {
            var sniNames = new[] { "sni-http-58", "sni-http-59", "sni-http-60", "sni-http-61", "sni-http-62" };

            this.logger.Information($"Total SNI's: {sniNames.Count()}");

            var notFoundCount = 0;

            var sslCertificates = await this.fortiWebService.GetLocalCertificatesAsync();
            var frontends = await this.domainService.GetFrontendsAsync();

            foreach (var sniName in sniNames)
            {
                var sniItem = await this.fortiWebService.GetServerNameIndicationsAsync(sniName);
                var results = sniItem.Results.ToList();

                this.logger.Information($"Item count in {sniName}: {results.Count}");

                var policyName = sniName.Replace("sni-http", "pol-http");
                var policyItem = await this.fortiWebService.GetServerPolicyRulesAsync(policyName);

                this.logger.Information($"Item count in {policyName}: {policyItem.Results.Count}");

                foreach (var item in results)
                {
                    this.logger.Information($"Checking domain: {item.Domain}");
                    this.logger.Information($"Certificate name: {item.LocalCert}");

                    var frontendItem = frontends.Find(x => x.SubDomains.Any(y => y.Name == item.Domain));
                    var frontendFound = frontendItem != null;
                    var backendName = frontendFound ? frontendItem.Backend.Name : item.Domain;
                    var contentRoutingPolicyName = this.GetContentRoutingName(backendName);

                    if (!frontendFound)
                    {
                        this.logger.Information($"Frontend not found: {item.Domain}");
                    }

                    var contentPolicyItem = policyItem.Results.Find(x => x.ContentRoutingPolicyName == contentRoutingPolicyName);
                    var contentPolicyFound = contentPolicyItem != null;

                    if (!contentPolicyFound)
                    {
                        this.logger.Information($"Content routing policy not found: {contentRoutingPolicyName}");
                    }

                    var contentRoutingRules = await this.fortiWebService.GetContentRoutingRulesAsync(contentRoutingPolicyName);
                    var contentRoutingRule = contentRoutingRules.Results.Find(x => x.MatchExpression == item.Domain);
                    var contentRoutingRuleFound = contentRoutingRule != null;

                    if (!contentRoutingRuleFound)
                    {
                        this.logger.Information($"Content routing rule not found: {item.Domain} ({contentRoutingPolicyName})");
                    }

                    var sslCertificate = sslCertificates.Results.Find(x => x.Name == item.LocalCert);
                    var validCertificate = sslCertificate.ValidTo > DateTime.Now;

                    if (!validCertificate)
                    {
                        this.logger.Information($"Certificate expired: {item.LocalCert} ({sslCertificate.ValidTo})");
                    }

                    if (!frontendFound && !contentPolicyFound && !contentRoutingRuleFound && !validCertificate)
                    {
                        this.logger.Warning($"DeleteServerNameIndicationAsync");
                        this.logger.Warning($"DeleteLocalCertificateAsync");

                        await this.fortiWebService.DeleteServerNameIndicationAsync(sniName, item.Id);
                        await this.fortiWebService.DeleteLocalCertificateAsync(item.LocalCert);
                    }
                    else if (!frontendFound && !contentPolicyFound && !contentRoutingRuleFound && validCertificate)
                    {
                        this.logger.Warning($"DeleteServerNameIndicationAsync");

                        await this.fortiWebService.DeleteServerNameIndicationAsync(sniName, item.Id);
                    }
                    else if (!frontendFound && contentPolicyFound && contentRoutingRuleFound)
                    {
                        this.logger.Warning($"FrontendFound not found, but contentPolicyFound and contentRoutingRuleFound found.");

                        //this.logger.Warning($"DeleteContentRoutingRuleAsync");
                        //this.logger.Warning($"DeleteContentRoutingAsync");
                        //this.logger.Warning($"DeleteServerNameIndicationAsync");
                        //this.logger.Warning($"DeleteLocalCertificateAsync");

                        // await this.fortiWebService.DeleteContentRoutingRuleAsync(contentRoutingPolicyName, contentRoutingRule.Id);
                        // await this.fortiWebService.DeleteContentRoutingAsync(contentRoutingPolicyName);
                        // await this.fortiWebService.DeleteServerNameIndicationAsync(sniName, item.Id);
                        // await this.fortiWebService.DeleteLocalCertificateAsync(item.Name);
                    }

                    //await this.fortiWebService.DeleteContentRoutingRuleAsync(contentRoutingPolicyName, contentRoutingRule.Id);

                    // ServerPolicy controleren op content routing
                    // Eventueel ContentRouting verwijderen uit Policy

                    //var checkContentRoutingRules = await this.fortiWebService.GetContentRoutingRulesAsync(contentRoutingPolicyName);
                    //contentRoutingRules.Results.Count
                    //await this.fortiWebService.DeleteContentRoutingAsync(contentRoutingPolicyName);

                    this.logger.Information($"-----------------------------------------------------------------");
                }

                this.logger.Information($"Items not found in {policyName}: {notFoundCount}");
            }

            return true;
        }

        private string GetContentRoutingName(string domain)
        {
            var slug = domain
               .Replace("www.", "")
               .Replace(".", " ")
               .Slugify();

            return $"{Settings.FortiWebContentRoutingPrefix}-{slug}";
        }
    }
}