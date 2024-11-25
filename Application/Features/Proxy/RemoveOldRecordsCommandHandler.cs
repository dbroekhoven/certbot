using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cre8ion.Database;
using Cre8ion.Database.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DatabaseEntities;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class RemoveOldRecordsCommandHandler : IRequestHandler<RemoveOldRecordsCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly DatabaseContext dbContext;

        public RemoveOldRecordsCommandHandler(DatabaseLogger logger, DatabaseContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public async Task<bool> Handle(RemoveOldRecordsCommand command, CancellationToken cancellationToken)
        {
            var lastMonth = System.DateTime.Today.AddMonths(-1);

            this.logger.Information($"Get archived Backends.");

            var archiveBackends = await this.dbContext.Set<Backend>()
                .IgnoreQueryFilters()
                .Where(x => x.Status == EntityStatus.Archive)
                .Where(x => x.DateTime < lastMonth)
                .ToListAsync();

            this.logger.Information($"Get archived Frontends.");

            var archiveFrontends = await this.dbContext.Set<Frontend>()
                .IgnoreQueryFilters()
                .Where(x => x.Status == EntityStatus.Archive)
                .Where(x => x.DateTime < lastMonth)
                .ToListAsync();

            this.logger.Information($"Get archived SslAuthorizations.");

            var archiveSslAuthorizations = await this.dbContext.Set<SslAuthorization>()
                .IgnoreQueryFilters()
                .Where(x => x.Status == EntityStatus.Approval)
                .Where(x => x.DateTime < lastMonth)
                .ToListAsync();

            this.logger.Information($"Get archived SslOrders.");

            var archiveSslOrders = await this.dbContext.Set<SslOrder>()
                .IgnoreQueryFilters()
                .Where(x => x.Status == EntityStatus.Archive)
                .Where(x => x.DateTime < lastMonth)
                .ToListAsync();

            var archiveSslOrderIds = archiveSslOrders
                .Select(x => x.Id)
                .ToList();

            this.logger.Information($"Get archived SslOrderData.");

            var archiveSslOrderData = await this.dbContext.Set<SslOrderData>()
                .IgnoreQueryFilters()
                .Where(x => archiveSslOrderIds.Contains(x.SslOrderId))
                .Where(x => x.DateTime < lastMonth)
                .ToListAsync();

            this.logger.Information($"Get archived SubDomains.");

            var archiveSubDomains = await this.dbContext.Set<SubDomain>()
                .IgnoreQueryFilters()
                .Where(x => x.Status == EntityStatus.Archive)
                .Where(x => x.DateTime < lastMonth)
                .ToListAsync();

            this.logger.Information($"Total archived Backends: {archiveBackends.Count}");
            this.logger.Information($"Total archived Frontends: {archiveFrontends.Count}");
            this.logger.Information($"Total archived SslAuthorizations: {archiveSslAuthorizations.Count}");
            this.logger.Information($"Total archived SslOrders: {archiveSslOrders.Count}");
            this.logger.Information($"Total archived SslOrderData: {archiveSslOrderData.Count}");
            this.logger.Information($"Total archived SubDomains: {archiveSubDomains.Count}");

            this.dbContext.RemoveRange(archiveSslAuthorizations);
            this.dbContext.SaveChanges();

            this.dbContext.RemoveRange(archiveSslOrderData);
            this.dbContext.SaveChanges();

            this.dbContext.RemoveRange(archiveSslOrders);
            this.dbContext.SaveChanges();

            this.dbContext.RemoveRange(archiveSubDomains);
            this.dbContext.SaveChanges();

            this.dbContext.RemoveRange(archiveFrontends);
            this.dbContext.SaveChanges();

            this.dbContext.RemoveRange(archiveBackends);
            this.dbContext.SaveChanges();

            return true;
        }
    }
}