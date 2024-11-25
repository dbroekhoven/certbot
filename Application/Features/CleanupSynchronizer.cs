using System;
using System.Threading.Tasks;
using Application.Features.Proxy;
using Cre8ion.Common;
using Cre8ion.Features.Synchronization;
using MediatR;
using Shared.Services;

namespace Application.Features
{
    public class CleanupSynchronizer : ISynchronizer, IService
    {
        private readonly DatabaseLogger logger;
        private readonly IMediator mediator;
        private readonly DomainService domainService;

        public CleanupSynchronizer(DatabaseLogger logger, IMediator mediator, DomainService domainService)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.domainService = domainService;
        }

        public async Task<string> SynchronizeAsync(DateTime? lastSyncDate)
        {
            var status = await this.ProcessStatusAsync();
            if (!status)
            {
                return string.Empty;
            }

            await this.ProcessCertificateCleanupAsync();
            await this.ProcessOldRecordsCleanupAsync();
            await this.ProcessOldSniCleanupAsync();

            return "Done!";
        }

        public async Task<bool> ProcessStatusAsync()
        {
            var proxyStatus = await this.mediator.Send(new ProxyStatusQuery());
            if (!proxyStatus)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ProcessCertificateCleanupAsync()
        {
            await this.mediator.Send(new RemoveUnusedCertificatesCommand());

            return true;
        }

        public async Task<bool> ProcessOldRecordsCleanupAsync()
        {
            await this.mediator.Send(new RemoveOldRecordsCommand());

            return true;
        }

        public async Task<bool> ProcessOldSniCleanupAsync()
        {
            //await this.mediator.Send(new RemoveOldSniCommand());

            return true;
        }
    }
}