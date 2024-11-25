using System;
using System.Threading.Tasks;
using Application.Features.Firewall;
using Cre8ion.Common;
using Cre8ion.Features.Synchronization;
using MediatR;

namespace Application.Features
{
    public class WhitelistSynchronizer : ISynchronizer, IService
    {
        private readonly IMediator mediator;

        public WhitelistSynchronizer(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<string> SynchronizeAsync(DateTime? lastSyncDate)
        {
            await this.mediator.Send(new WhitelistAddressesCommand());

            return "Done!";
        }
    }
}