using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cre8ion.Database;
using MediatR;
using Shared.DatabaseEntities;
using Shared.Extensions;
using Shared.Models;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class WildcardServerNameIndicationCommandHandler : IRequestHandler<WildcardServerNameIndicationCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiWebService fortiWebService;

        public WildcardServerNameIndicationCommandHandler(DatabaseLogger logger, FortiWebService fortiWebService)
        {
            this.logger = logger;
            this.fortiWebService = fortiWebService;
        }

        public async Task<bool> Handle(WildcardServerNameIndicationCommand command, CancellationToken cancellationToken)
        {
            if (command.Frontend.IsDeleted)
            {
                return await this.DeleteAsync(command);
            }

            return await this.CreateOrUpdateAsync(command);
        }

        private async Task<bool> DeleteAsync(WildcardServerNameIndicationCommand command)
        {
            var serverNameIndicationName = command.Frontend.Backend.GetServerNameIndication();

            var results = new List<bool>();

            var subDomains = command.Frontend.SubDomains
                .Where(x => x.Status == EntityStatus.Published)
                .ToList();

            foreach (var subDomain in subDomains)
            {
                var check = await this.ExistsAsync(subDomain, subDomain.Name, serverNameIndicationName);
                if (check.Exists)
                {
                    var result = await this.DeleteAsync(subDomain, subDomain.Name, serverNameIndicationName, check.Id);

                    results.Add(result);
                }

                subDomain.Status = EntityStatus.Archive;
            }

            return true;
        }

        private async Task<bool> DeleteAsync(SubDomain subDomain, string domainName, string serverNameIndication, string sniId)
        {
            var create = await this.fortiWebService.DeleteServerNameIndicationAsync(serverNameIndication, sniId);
            this.logger.Debug(subDomain, $"DeleteServerNameIndicationAsync: {create.Json}");

            if (create.HasError)
            {
                this.logger.Error(subDomain, $"Server name indication failed: {domainName} with errorcode {create.Result.ErrorCode}");

                return false;
            }

            this.logger.Information(subDomain, $"Server name indication deleted for {domainName} on {serverNameIndication}");

            return true;
        }

        private async Task<bool> CreateOrUpdateAsync(WildcardServerNameIndicationCommand command)
        {
            var certificateName = command.Frontend.SslWildcard.GetCertificateName();
            var intermediateGroup = command.Frontend.SslWildcard.CaRootGroup;
            var serverNameIndicationName = command.Frontend.Backend.GetServerNameIndication();

            var groupExists = await this.ExistsIntermediateGroupAsync(intermediateGroup);
            if (!groupExists)
            {
                this.logger.Warning(command.Frontend, $"Intermediate group {intermediateGroup} does not exists");
                this.logger.Warning(command.Frontend.SslWildcard, $"Intermediate group {intermediateGroup} does not exists");

                return false;
            }

            var results = new List<bool>();

            var subDomains = command.Frontend.SubDomains
                .Where(x => x.Status == EntityStatus.Published)
                .ToList();

            foreach (var subDomain in subDomains)
            {
                this.logger.Information(subDomain, $"Server name indication for: {command.Frontend.Name} named {certificateName}");

                var check = await this.ExistsAsync(subDomain, subDomain.Name, serverNameIndicationName);
                if (check.Exists)
                {
                    var result = await this.UpdateAsync(subDomain, subDomain.Name, serverNameIndicationName, certificateName, intermediateGroup, check.Id);

                    results.Add(result);
                }
                else
                {
                    var result = await this.CreateAsync(subDomain, subDomain.Name, serverNameIndicationName, certificateName, intermediateGroup);

                    results.Add(result);
                }
            }

            if (results.All(x => x))
            {
                command.Frontend.ChangedWildcard = false;
                command.Frontend.HasWildcard = true;
            }

            return true;
        }

        private async Task<bool> ExistsIntermediateGroupAsync(string intermediateGroup)
        {
            var intermediateGroups = await this.fortiWebService.GetIntermediateGroupsAsync();

            return intermediateGroups.Results.Any(x => x.Name == intermediateGroup);
        }

        private async Task<SniResult> ExistsAsync(SubDomain subDomain, string domainName, string serverNameIndication)
        {
            var serverNameIndications = await this.fortiWebService.GetServerNameIndicationsAsync(serverNameIndication);
            var result = serverNameIndications.Results.Find(x => x.Domain == domainName);

            if (result == null)
            {
                this.logger.Information(subDomain, $"Server name indication {domainName} does not exists on {serverNameIndication}");

                return new SniResult
                {
                    Exists = false
                };
            }

            this.logger.Information(subDomain, $"Server name indication {domainName} exists on {serverNameIndication}");

            return new SniResult
            {
                Exists = true,
                Id = result.Id
            };
        }

        private async Task<bool> CreateAsync(SubDomain subDomain, string domainName, string serverNameIndication, string certificateName, string intermediateGroup)
        {
            var create = await this.fortiWebService.CreateServerNameIndicationAsync(serverNameIndication, domainName, certificateName, intermediateGroup);
            this.logger.Debug(subDomain, $"CreateServerNameIndicationAsync: {create.Json}");

            if (create.HasError)
            {
                this.logger.Error(subDomain, $"Server name indication failed: {domainName} with errorcode {create.Result.ErrorCode}");

                return false;
            }

            this.logger.Information(subDomain, $"Server name indication created for {domainName} on {serverNameIndication}");

            return true;
        }

        private async Task<bool> UpdateAsync(SubDomain subDomain, string domainName, string serverNameIndication, string certificateName, string intermediateGroup, string sniId)
        {
            var create = await this.fortiWebService.UpdateServerNameIndicationAsync(sniId, serverNameIndication, domainName, certificateName, intermediateGroup);
            this.logger.Debug(subDomain, $"UpdateServerNameIndicationAsync: {create.Json}");

            if (create.HasError)
            {
                this.logger.Error(subDomain, $"Server name indication failed: {domainName} with errorcode {create.Result.ErrorCode}");

                return false;
            }

            this.logger.Information(subDomain, $"Server name indication updated for {domainName} on {serverNameIndication}");

            return true;
        }
    }
}