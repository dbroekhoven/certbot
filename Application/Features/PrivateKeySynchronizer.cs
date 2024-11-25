using System;
using System.Threading.Tasks;
using Application.Features.Proxy;
using Cre8ion.Common;
using Cre8ion.Features.Synchronization;
using MediatR;
using Shared.Services;

namespace Application.Features
{
    public class PrivateKeySynchronizer : ISynchronizer, IService
    {
        private readonly DatabaseLogger logger;
        private readonly IMediator mediator;
        private readonly DomainService domainService;

        public PrivateKeySynchronizer(DatabaseLogger logger, IMediator mediator, DomainService domainService)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.domainService = domainService;
        }

        public async Task<string> SynchronizeAsync(DateTime? lastSyncDate)
        {
            await this.GenerateNewPrivateKeys();

            return "Done!";
        }

        public async Task<bool> GenerateNewPrivateKeys()
        {
            await this.mediator.Send(new GenerateNewPrivateKeysCommand());

            return true;
        }
    }
}