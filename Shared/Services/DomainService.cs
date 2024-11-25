using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cre8ion.Common;
using Cre8ion.Database;
using Cre8ion.Database.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Shared.DatabaseEntities;

namespace Shared.Services
{
    public class DomainService : IService
    {
        private readonly DatabaseContext dbContext;

        public DomainService(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SaveChangesAsync()
        {
            await this.dbContext.SaveChangesAsync();
        }

        public async Task<SslOrderData> GetOrderDataAsync(int sslOrderId)
        {
            return await this.dbContext.Set<SslOrderData>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.SslOrderId == sslOrderId)
                .OrderBy(x => x.Id)
                .LastOrDefaultAsync();
        }

        public async Task<List<SslWildcard>> GetWildcardsAsync()
        {
            return await this.dbContext.Set<SslWildcard>()
                .ToListAsync();
        }

        public async Task<SslWildcard> GetWildcardAsync(int id)
        {
            return await this.dbContext.Set<SslWildcard>()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Backend>> GetBackendsAsync(bool deleted)
        {
            return await this.dbContext.Set<Backend>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.IsDeleted == deleted)
                .ToListAsync();
        }

        public async Task<List<Backend>> GetBackendsAsync(string domainName)
        {
            return await this.dbContext.Set<Backend>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => !x.IsDeleted)
                .Where(x => x.Name == domainName)
                .ToListAsync();
        }

        public async Task<List<Backend>> GetBackendsAsync(bool deleted, CdnServer cdn)
        {
            if (cdn == CdnServer.Default)
            {
                return await this.dbContext.Set<Backend>()
                    .Where(x => x.Status == EntityStatus.Published)
                    .Where(x => x.IsDeleted == deleted)
                    .Where(x => x.IsCdn && !x.Name.EndsWith("orangebag.nl"))
                    .ToListAsync();
            }

            if (cdn == CdnServer.Orangebag)
            {
                return await this.dbContext.Set<Backend>()
                    .Where(x => x.Status == EntityStatus.Published)
                    .Where(x => x.IsDeleted == deleted)
                    .Where(x => x.IsCdn && x.Name.EndsWith("orangebag.nl"))
                    .ToListAsync();
            }

            return await this.dbContext.Set<Backend>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.IsDeleted == deleted)
                .Where(x => !x.IsCdn)
                .ToListAsync();
        }

        public async Task<List<Frontend>> GetFrontendsAsync(string domainName)
        {
            return await this.dbContext.Set<Frontend>()
                .AsSplitQuery()
                .Include(x => x.Backend)
                .Include(x => x.SubDomains)
                .Include(x => x.SslWildcard)
                .Include(x => x.SslOrders)
                    .ThenInclude(x => x.Authorizations)
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.Backend.Status == EntityStatus.Published)
                .Where(x => x.Name == domainName)
                .ToListAsync();
        }

        public async Task<List<Frontend>> GetFrontendsAsync()
        {
            return await this.dbContext.Set<Frontend>()
                .AsSplitQuery()
                .Include(x => x.Backend)
                .Include(x => x.SubDomains)
                .Include(x => x.SslWildcard)
                .Include(x => x.SslOrders)
                    .ThenInclude(x => x.Authorizations)
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.Backend.Status == EntityStatus.Published)
                .ToListAsync();
        }

        public async Task<List<Frontend>> GetFrontendsAsync(CdnServer cdn)
        {
            if (cdn == CdnServer.Default)
            {
                return await this.dbContext.Set<Frontend>()
                    .AsSplitQuery()
                    .Include(x => x.Backend)
                    .Include(x => x.SubDomains)
                    .Include(x => x.SslWildcard)
                    .Include(x => x.SslOrders)
                        .ThenInclude(x => x.Authorizations)
                    .Where(x => x.Status == EntityStatus.Published)
                    .Where(x => x.Backend.Status == EntityStatus.Published)
                    .Where(x => x.Backend.IsCdn && !x.Name.EndsWith("orangebag.nl"))
                    .ToListAsync();
            }

            if (cdn == CdnServer.Orangebag)
            {
                return await this.dbContext.Set<Frontend>()
                    .AsSplitQuery()
                    .Include(x => x.Backend)
                    .Include(x => x.SubDomains)
                    .Include(x => x.SslWildcard)
                    .Include(x => x.SslOrders)
                        .ThenInclude(x => x.Authorizations)
                    .Where(x => x.Status == EntityStatus.Published)
                    .Where(x => x.Backend.Status == EntityStatus.Published)
                    .Where(x => x.Backend.IsCdn && x.Name.EndsWith("orangebag.nl"))
                    .ToListAsync();
            }

            return await this.dbContext.Set<Frontend>()
                .AsSplitQuery()
                .Include(x => x.Backend)
                .Include(x => x.SubDomains)
                .Include(x => x.SslWildcard)
                .Include(x => x.SslOrders)
                    .ThenInclude(x => x.Authorizations)
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.Backend.Status == EntityStatus.Published)
                .Where(x => !x.Backend.IsCdn)
                .ToListAsync();
        }

        public async Task<List<Frontend>> GetFrontendsWithLetsEncryptAsync()
        {
            return await this.dbContext.Set<Frontend>()
                .AsSplitQuery()
                .Include(x => x.Backend)
                .Include(x => x.SubDomains)
                .Include(x => x.SslWildcard)
                .Include(x => x.SslOrders)
                    .ThenInclude(x => x.Authorizations)
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.Backend.Status == EntityStatus.Published)
                .Where(x => x.UseLetsEncrypt)
                .Where(x => x.SslOrders.Any(y => y.Status == EntityStatus.Published) && x.SslOrders.Where(y => y.Status == EntityStatus.Published).Max(y => y.NotAfter.Value.Date).AddDays(Settings.LetsEncryptRenewalDays) <= DateTime.Today)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }

        public async Task<List<Address>> GetBlockedIpv4AddressesAsync()
        {
            return await this.dbContext.Set<Address>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.Value.Contains(".") && !x.Value.Contains(":"))
                .Where(x => x.IsBlocked)
                .ToListAsync();
        }

        public async Task<List<Address>> GetBlockedIpv6AddressesAsync()
        {
            return await this.dbContext.Set<Address>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.Value.Contains(":") && !x.Value.Contains("."))
                .Where(x => x.IsBlocked)
                .ToListAsync();
        }

        public async Task<List<Address>> GetFtpAddressesAsync()
        {
            return await this.dbContext.Set<Address>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.IsFtp)
                .ToListAsync();
        }

        public async Task<List<WhitelistAddress>> GetWhitelistAddressesAsync()
        {
            return await this.dbContext.Set<WhitelistAddress>()
                .Where(x => x.Status == EntityStatus.Published || x.Status == EntityStatus.Archive)
                .ToListAsync();
        }
    }
}