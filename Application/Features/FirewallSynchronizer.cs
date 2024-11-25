using System;
using System.Threading.Tasks;
using Application.Features.Firewall;
using Cre8ion.Common;
using Cre8ion.Features.Synchronization;
using MediatR;

namespace Application.Features
{
    public class FirewallSynchronizer : ISynchronizer, IService
    {
        private readonly IMediator mediator;

        public FirewallSynchronizer(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<string> SynchronizeAsync(DateTime? lastSyncDate)
        {
            var status = await this.ProcessStatusAsync();
            if (!status)
            {
                return string.Empty;
            }

            await this.ProcessFirewallAddressesAsync();

            return "Done!";
        }

        public async Task<bool> ProcessStatusAsync()
        {
            var firewallStatus = await this.mediator.Send(new FirewallStatusQuery());
            if (!firewallStatus)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ProcessFirewallAddressesAsync()
        {
            await this.mediator.Send(new BlockAddressesCommand());
            await this.mediator.Send(new FtpAddressesCommand());
            await this.mediator.Send(new WhitelistAddressesCommand());

            return true;
        }
    }
}