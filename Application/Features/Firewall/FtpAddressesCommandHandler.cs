using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Extensions;
using Shared.FortiGateModels;
using Shared.Services;

namespace Application.Features.Firewall
{
    public class FtpAddressesCommandHandler : IRequestHandler<FtpAddressesCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiGateService fortiGateService;
        private readonly DomainService domainService;

        public FtpAddressesCommandHandler(DatabaseLogger logger, FortiGateService fortiGateService, DomainService domainService)
        {
            this.logger = logger;
            this.fortiGateService = fortiGateService;
            this.domainService = domainService;
        }

        public async Task<bool> Handle(FtpAddressesCommand command, CancellationToken cancellationToken)
        {
            var addresses = await this.fortiGateService.GetIpv4AddressListAsync();

            var allowed = await this.domainService.GetFtpAddressesAsync();
            var unprocessed = allowed
                .Where(x => !x.HasAddress)
                .ToList();

            this.logger.Information($"Total ftp: {allowed.Count}");

            foreach (var item in unprocessed)
            {
                this.logger.Information(item, $"Processing {item.Name}");

                var name = item.GetFtpName();
                var found = addresses.Results.Any(x => x.Name == name);
                if (found)
                {
                    this.logger.Warning(item, $"Address {item.Name} already exists");

                    item.HasAddress = true;

                    continue;
                }

                var request = new FortiGateAddressCreate
                {
                    Interface = new FortiGateAddressCreateInterface
                    {
                        Port = "port4"
                    },
                    Color = "4",
                    Name = name,
                    Subnet = item.Value
                };

                var result = await this.fortiGateService.CreateIpv4AddressAsync(request);

                if (result.Status != "success")
                {
                    this.logger.Debug(item, $"CreateAddress: {result.Json}");

                    this.logger.Error($"Address {item.Name} failed with status {result.Status}");

                    continue;
                }

                this.logger.Information($"Address {item.Name} with value {item.Value} created");

                item.HasAddress = true;
            }

            var members = allowed
                .Where(x => x.HasAddress)
                .Where(x => !x.IsDeleted)
                .Select(x => new FortiGateAddressListMember
                {
                    Name = x.GetFtpName()
                })
                .ToList();

            var data = new FortiGateAddressListUpdate
            {
                AllowRouting = "disable",
                Category = "default",
                Color = "4",
                Exclude = "disable",
                FabricObject = "disable",
                Members = members,
                Name = "ftp",
                Type = "default"
            };

            var update = await this.fortiGateService.UpdateIpv4AddressListAsync("ftp", data);

            var deleted = allowed
                .Where(x => x.IsDeleted)
                .ToList();

            foreach (var item in deleted)
            {
                this.logger.Information(item, $"Processing {item.Name}");

                var name = item.GetFtpName();
                var found = addresses.Results.Any(x => x.Name == name);
                if (!found)
                {
                    this.logger.Warning(item, $"Address {item.Name} does not exists");

                    item.HasAddress = false;
                    item.Status = Cre8ion.Database.EntityStatus.Archive;

                    continue;
                }

                var result = await this.fortiGateService.DeleteIpv4AddressAsync(name);

                if (result.Status != "success")
                {
                    this.logger.Debug(item, $"DeleteAddress: {result.Json}");

                    this.logger.Error(item, $"Address {item.Name} failed with status {result.Status}");

                    continue;
                }

                this.logger.Information(item, $"Address {item.Name} with value {item.Value} deleted");

                item.HasAddress = false;
                item.Status = Cre8ion.Database.EntityStatus.Archive;
            }

            return true;
        }
    }
}