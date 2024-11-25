using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Extensions;
using Shared.FortiGateModels;
using Shared.Services;

namespace Application.Features.Firewall
{
    public class BlockAddressesCommandHandler : IRequestHandler<BlockAddressesCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiGateService fortiGateService;
        private readonly DomainService domainService;

        public BlockAddressesCommandHandler(DatabaseLogger logger, FortiGateService fortiGateService, DomainService domainService)
        {
            this.logger = logger;
            this.fortiGateService = fortiGateService;
            this.domainService = domainService;
        }

        public async Task<bool> Handle(BlockAddressesCommand command, CancellationToken cancellationToken)
        {
            await this.HandleIpv4();
            await this.HandleIpv6();

            return true;
        }

        public async Task<bool> HandleIpv4()
        {
            var addresses = await this.fortiGateService.GetIpv4AddressListAsync();

            this.logger.Debug($"GetIpv4AddressList: {addresses.Json}");

            var blocked = await this.domainService.GetBlockedIpv4AddressesAsync();
            var unprocessed = blocked
                .Where(x => !x.HasAddress)
                .ToList();

            this.logger.Information($"Total blocked: {blocked.Count}");
            this.logger.Information($"Total unprocessed: {unprocessed.Count}");

            foreach (var item in unprocessed)
            {
                this.logger.Information(item, $"Processing {item.Name}");

                var name = item.GetBlockName();
                var found = addresses.Results.Any(x => x.Name == name);
                if (found)
                {
                    this.logger.Warning(item, $"Address {name} already exists");

                    item.HasAddress = true;

                    continue;
                }

                var request = new FortiGateAddressCreate
                {
                    Interface = new FortiGateAddressCreateInterface
                    {
                        Port = "port4"
                    },
                    Color = "9",
                    Name = name,
                    Subnet = item.Value
                };

                var result = await this.fortiGateService.CreateIpv4AddressAsync(request);

                if (result.Status != "success")
                {
                    this.logger.Debug(item, $"CreateAddress: {result.Json}");

                    this.logger.Error(item, $"Address {name} failed with status {result.Status}");

                    continue;
                }

                this.logger.Information(item, $"Address {name} with value {item.Value} created");

                item.HasAddress = true;
            }

            var members = blocked
                .Where(x => x.HasAddress)
                .Where(x => !x.IsDeleted)
                .Select(x => new FortiGateAddressListMember
                {
                    Name = x.GetBlockName()
                })
                .ToList();

            this.logger.Information($"Total members: {members.Count}");

            var data = new FortiGateAddressListUpdate
            {
                AllowRouting = "disable",
                Category = "default",
                Color = "9",
                Exclude = "disable",
                FabricObject = "disable",
                Members = members,
                Name = "block",
                Type = "default"
            };

            var update = await this.fortiGateService.UpdateIpv4AddressListAsync("block", data);

            this.logger.Debug($"UpdateAddressList: {update.Json}");

            var deleted = blocked
                .Where(x => x.IsDeleted)
                .ToList();

            foreach (var item in deleted)
            {
                this.logger.Information(item, $"Processing {item.Name}");

                var name = item.GetBlockName();
                var found = addresses.Results.Any(x => x.Name == name);
                if (!found)
                {
                    this.logger.Warning(item, $"Address {name} does not exists");

                    item.HasAddress = false;
                    item.Status = Cre8ion.Database.EntityStatus.Archive;

                    continue;
                }

                var result = await this.fortiGateService.DeleteIpv4AddressAsync(name);

                if (result.Status != "success")
                {
                    this.logger.Debug(item, $"DeleteAddress: {result.Json}");

                    this.logger.Error(item, $"Address {name} failed with status {result.Status}");

                    continue;
                }

                this.logger.Information(item, $"Address {name} with value {item.Value} deleted");

                item.HasAddress = false;
                item.Status = Cre8ion.Database.EntityStatus.Archive;
            }

            return true;
        }

        public async Task<bool> HandleIpv6()
        {
            var addresses = await this.fortiGateService.GetIpv6AddressListAsync();

            this.logger.Debug($"GetIpv6AddressList: {addresses.Json}");

            var blocked = await this.domainService.GetBlockedIpv6AddressesAsync();
            var unprocessed = blocked
                .Where(x => !x.HasAddress)
                .ToList();

            this.logger.Information($"Total blocked: {blocked.Count}");
            this.logger.Information($"Total unprocessed: {unprocessed.Count}");

            foreach (var item in unprocessed)
            {
                this.logger.Information(item, $"Processing {item.Name}");

                var name = item.GetBlockName();
                var found = addresses.Results.Any(x => x.Name == name);
                if (found)
                {
                    this.logger.Warning(item, $"Address {name} already exists");

                    item.HasAddress = true;

                    continue;
                }

                var request = new FortiGateAddressCreate
                {
                    Interface = new FortiGateAddressCreateInterface
                    {
                        Port = "port4"
                    },
                    Color = "9",
                    Name = name,
                    Address6 = item.Value
                };

                var result = await this.fortiGateService.CreateIpv6AddressAsync(request);

                if (result.Status != "success")
                {
                    this.logger.Debug(item, $"CreateAddress: {result.Json}");

                    this.logger.Error(item, $"Address {name} failed with status {result.Status}");

                    continue;
                }

                this.logger.Information(item, $"Address {name} with value {item.Value} created");

                item.HasAddress = true;
            }

            var members = blocked
                .Where(x => x.HasAddress)
                .Where(x => !x.IsDeleted)
                .Select(x => new FortiGateAddressListMember
                {
                    Name = x.GetBlockName()
                })
                .ToList();

            this.logger.Information($"Total members: {members.Count}");

            var data = new FortiGateAddressListUpdate
            {
                AllowRouting = "disable",
                Category = "default",
                Color = "9",
                Exclude = "disable",
                FabricObject = "disable",
                Members = members,
                Name = "block",
                Type = "default"
            };

            var update = await this.fortiGateService.UpdateIpv6AddressListAsync("block", data);

            this.logger.Debug($"UpdateAddressList: {update.Json}");

            var deleted = blocked
                .Where(x => x.IsDeleted)
                .ToList();

            foreach (var item in deleted)
            {
                this.logger.Information(item, $"Processing {item.Name}");

                var name = item.GetBlockName();
                var found = addresses.Results.Any(x => x.Name == name);
                if (!found)
                {
                    this.logger.Warning(item, $"Address {item.Name} does not exists");

                    item.HasAddress = false;
                    item.Status = Cre8ion.Database.EntityStatus.Archive;

                    continue;
                }

                var result = await this.fortiGateService.DeleteIpv6AddressAsync(name);

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