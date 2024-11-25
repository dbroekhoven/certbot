using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cre8ion.Database;
using MediatR;
using Shared.Extensions;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class ArchiveOldSslOrdersCommandHandler : IRequestHandler<ArchiveOldSslOrdersCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly DomainService domainService;

        public ArchiveOldSslOrdersCommandHandler(DatabaseLogger logger, DomainService domainService)
        {
            this.logger = logger;
            this.domainService = domainService;
        }

        public async Task<bool> Handle(ArchiveOldSslOrdersCommand command, CancellationToken cancellationToken)
        {
            var frontends = await this.domainService.GetFrontendsAsync();

            foreach (var frontend in frontends)
            {
                var finishedOrders = frontend.SslOrders
                    .Where(x => x.Status == EntityStatus.Published)
                    .OrderByDescending(x => x.NotAfter)
                    .ToList();

                if (finishedOrders.Count <= 1)
                {
                    continue;
                }

                this.logger.Information(frontend, $"Archive old orders for: {frontend.Name}");

                foreach (var order in finishedOrders)
                {
                    var first = finishedOrders.IndexOf(order) == 0;
                    if (first)
                    {
                        continue;
                    }

                    this.logger.Information(order, $"Archive old order for: {frontend.Name}, {order.GetCertificateName(frontend)}");

                    order.Status = EntityStatus.Archive;
                }
            }

            return true;
        }
    }
}