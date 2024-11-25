using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Extensions;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class VirtualAddressGroupCommandHandler : IRequestHandler<VirtualAddressGroupCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiWebService fortiWebService;

        public VirtualAddressGroupCommandHandler(DatabaseLogger logger, FortiWebService fortiWebService)
        {
            this.logger = logger;
            this.fortiWebService = fortiWebService;
        }

        public async Task<bool> Handle(VirtualAddressGroupCommand command, CancellationToken cancellationToken)
        {
            if (!command.Backend.UseIPv6 && !command.Backend.HasIPv6)
            {
                return true;
            }

            if (command.Backend.UseIPv6 && command.Backend.HasIPv6 && !command.Backend.IsDeleted)
            {
                return true;
            }

            this.logger.Information(command.Backend, $"Virtual IPv6 address for: {command.Backend.Name}");

            if (command.Backend.IsDeleted)
            {
                return await this.DeleteAsync(command);
            }

            if (!command.Backend.UseIPv6 && command.Backend.HasIPv6)
            {
                return await this.DeleteAsync(command);
            }

            return await this.CreateAsync(command);
        }

        public async Task<bool> CreateAsync(VirtualAddressGroupCommand command)
        {
            var vipName = command.Backend.GetIPv6VipName();
            var ipAddress = command.Backend.GetIPv6Address();
            var serverName = command.Backend.GetVirtualServerName();

            var allVirtualIPs = await this.fortiWebService.GetVirtualIPv6Async();
            if (allVirtualIPs.Results.Any(x => x.Name == vipName))
            {
                this.logger.Warning(command.Backend, $"Virtual IPv6 address failed: {vipName}, {ipAddress} already exists");
            }
            else
            {
                var result = await this.fortiWebService.CreateVirtualIPv6Async(vipName, ipAddress);
                this.logger.Debug(command.Backend, $"CreateVirtualIPv6Async: {result.Json}");

                this.logger.Information(command.Backend, $"Virtual IPv6 address created: {vipName}, {ipAddress}");

                var updateResult = await this.fortiWebService.UpdateVirtualServerAsync(serverName, vipName);
                this.logger.Debug(command.Backend, $"UpdateVirtualServerAsync: {updateResult.Json}");

                this.logger.Information(command.Backend, $"Virtual IPv6 group updated: {serverName}, {vipName}");
            }

            command.Backend.HasIPv6 = true;

            return true;
        }

        public async Task<bool> DeleteAsync(VirtualAddressGroupCommand command)
        {
            var vipName = command.Backend.GetIPv6VipName();
            var serverName = command.Backend.GetVirtualServerName();

            var virtualIps = await this.fortiWebService.GetVirtualServerVipsAsync(serverName);
            var virtualIp = virtualIps.Results.Find(x => x.Vip == vipName);

            if (virtualIp == null)
            {
                this.logger.Warning(command.Backend, $"Virtual IPv6 address deleted: {vipName} does not exists in {serverName}");
            }
            else
            {
                var result = await this.fortiWebService.DeleteVirtualServerVipAsync(serverName, virtualIp.Id);
                this.logger.Debug(command.Backend, $"DeleteVirtualServerVipAsync: {result.Json}");

                this.logger.Information(command.Backend, $"Virtual IPv6 {vipName} deleted from: {serverName}");
            }

            var allVirtualIPs = await this.fortiWebService.GetVirtualIPv6Async();
            virtualIp = virtualIps.Results.Find(x => x.Vip == vipName);

            if (virtualIp == null)
            {
                this.logger.Warning(command.Backend, $"Virtual IPv6 address failed: {vipName} does not exists anymore");
            }
            else
            {
                var result = await this.fortiWebService.DeleteVirtualIPv6Async(vipName);
                this.logger.Debug(command.Backend, $"DeleteVirtualIPv6Async: {result.Json}");

                this.logger.Information(command.Backend, $"Virtual IPv6 address deleted: {vipName}");
            }

            command.Backend.HasIPv6 = false;

            return true;
        }
    }
}