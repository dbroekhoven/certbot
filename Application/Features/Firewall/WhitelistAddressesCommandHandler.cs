using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.FortiWebModels;
using Shared.Services;

namespace Application.Features.Firewall
{
    public class WhitelistAddressesCommandHandler : IRequestHandler<WhitelistAddressesCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiWebService fortiWebService;
        private readonly DomainService domainService;

        public WhitelistAddressesCommandHandler(DatabaseLogger logger, FortiWebService fortiWebService, DomainService domainService)
        {
            this.logger = logger;
            this.fortiWebService = fortiWebService;
            this.domainService = domainService;
        }

        public async Task<bool> Handle(WhitelistAddressesCommand command, CancellationToken cancellationToken)
        {
            var whitelistMembers = await this.fortiWebService.GetIpListAsync("Test+Whitelist");

            var addresses = await this.domainService.GetWhitelistAddressesAsync();

            var published = addresses
                .Where(x => x.Status == Cre8ion.Database.EntityStatus.Published)
                .ToList();

            var unprocessed = addresses
                .Where(x => x.Status == Cre8ion.Database.EntityStatus.Published)
                .Where(x => !x.HasAddress)
                .ToList();

            var deleted = addresses
                .Where(x => x.Status == Cre8ion.Database.EntityStatus.Archive)
                .Where(x => x.HasAddress)
                .Where(x => !x.IsDeleted)
                .ToList();

            this.logger.Information($"Total whitelisted: {published.Count}");
            this.logger.Information($"Total unprocessed: {unprocessed.Count}");
            this.logger.Information($"Total deleted: {deleted.Count}");

            foreach (var item in unprocessed)
            {
                this.logger.Information($"Processing {item.Value}");

                var found = whitelistMembers.Results.Any(x => x.Ip == item.Value);
                if (found)
                {
                    this.logger.Warning($"Address {item.Value} already exists");

                    item.HasAddress = true;

                    continue;
                }

                var request = new FortiWebIpMemberCreate
                {
                    Data = new FortiWebIpMember
                    {
                        GroupType = "ip-string",
                        Ip = item.Value,
                        QType = 0,
                        Type = "trust-ip"
                    }
                };

                var result = await this.fortiWebService.CreateIpListAsync("Test+Whitelist", request);

                this.logger.Information($"Address {item.Value} with value {item.Value} created");

                item.HasAddress = true;
            }

            foreach (var item in deleted)
            {
                this.logger.Information($"Deleting {item.Value}");

                var member = whitelistMembers.Results.Find(x => x.Ip == item.Value);
                if (member == null)
                {
                    this.logger.Warning($"Address {item.Value} does not exists");

                    item.IsDeleted = true;
                    item.HasAddress = false;

                    continue;
                }

                var result = await this.fortiWebService.RemoveIpListAsync("Test+Whitelist", member);

                this.logger.Information($"Address {item.Value} with value {item.Value} deleted");

                item.IsDeleted = true;
                item.HasAddress = false;
            }

            return true;
        }
    }
}