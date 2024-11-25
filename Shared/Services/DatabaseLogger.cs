using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cre8ion.Common;
using Cre8ion.Database;
using Cre8ion.Database.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.DatabaseEntities;

namespace Shared.Services
{
    public class DatabaseLogger : IService
    {
        private readonly ILogger<Cre8ion.Startup> logger;
        private readonly DatabaseContext dbContext;

        public DatabaseLogger(ILogger<Cre8ion.Startup> logger, DatabaseContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public async Task<List<Log>> GetRecentLogsAsync()
        {
            var subjects = new[] { "Warning", "Error" };
            var date = DateTime.Today.AddDays(-5);

            return await this.dbContext.Set<Log>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => subjects.Contains(x.Subject))
                .Where(x => x.DateTime > date)
                .OrderByDescending(x => x.DateTime)
                .Take(10)
                .ToListAsync();
        }

        public async Task<List<Log>> GetReverseProxyLogsAsync()
        {
            var today = DateTime.Today;

            return await this.dbContext.Set<Log>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.DateTime.Date == today)
                .Where(x => x.AddressId == 0)
                .Where(x => x.Subject != "Debug")
                .OrderByDescending(x => x.DateTime)
                .ToListAsync();
        }

        public async Task<List<Log>> GetFirewallLogsAsync()
        {
            var from = DateTime.Today.AddDays(-2);

            return await this.dbContext.Set<Log>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.DateTime > from)
                .Where(x => x.AddressId > 0)
                .OrderByDescending(x => x.DateTime)
                .ToListAsync();
        }

        public async Task<List<Log>> GetLogsAsync(Backend backend)
        {
            return await this.dbContext.Set<Log>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.BackendId == backend.Id)
                .OrderByDescending(x => x.DateTime)
                .ToListAsync();
        }

        public async Task<List<Log>> GetLogsAsync(Address address)
        {
            return await this.dbContext.Set<Log>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.AddressId == address.Id)
                .OrderByDescending(x => x.DateTime)
                .ToListAsync();
        }

        public async Task<List<Log>> GetLogsAsync(Frontend frontend)
        {
            return await this.dbContext.Set<Log>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.FrontendId == frontend.Id)
                .OrderByDescending(x => x.DateTime)
                .ToListAsync();
        }

        public async Task<List<Log>> GetLogsAsync(SslWildcard sslWildcard)
        {
            return await this.dbContext.Set<Log>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.SslWildcardId == sslWildcard.Id)
                .OrderByDescending(x => x.DateTime)
                .ToListAsync();
        }

        public void Information(string message)
        {
            this.logger.LogInformation(message);

            var log = new Log
            {
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Information"
            };

            this.dbContext.Add(log);
        }

        public void Warning(string message)
        {
            this.logger.LogWarning(message);

            var log = new Log
            {
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Warning"
            };

            this.dbContext.Add(log);
        }

        public void Error(string message)
        {
            this.logger.LogError(message);

            var log = new Log
            {
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Error"
            };

            this.dbContext.Add(log);
        }

        public void Debug(string message)
        {
            this.logger.LogDebug(message);

            var log = new Log
            {
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Debug"
            };

            this.dbContext.Add(log);
        }

        public void Information(SslWildcard wildcard, string message)
        {
            this.logger.LogInformation(message);

            var log = new Log
            {
                SslWildcardId = wildcard.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Information"
            };

            this.dbContext.Add(log);
        }

        public void Warning(SslWildcard wildcard, string message)
        {
            this.logger.LogWarning(message);

            var log = new Log
            {
                SslWildcardId = wildcard.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Warning"
            };

            this.dbContext.Add(log);
        }

        public void Error(SslWildcard wildcard, string message)
        {
            this.logger.LogError(message);

            var log = new Log
            {
                SslWildcardId = wildcard.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Error"
            };

            this.dbContext.Add(log);
        }


        public void Information(Address address, string message)
        {
            this.logger.LogInformation(message);

            var log = new Log
            {
                AddressId = address.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Information"
            };

            this.dbContext.Add(log);
        }

        public void Warning(Address address, string message)
        {
            this.logger.LogWarning(message);

            var log = new Log
            {
                AddressId = address.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Warning"
            };

            this.dbContext.Add(log);
        }

        public void Error(Address address, string message)
        {
            this.logger.LogError(message);

            var log = new Log
            {
                AddressId = address.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Error"
            };

            this.dbContext.Add(log);
        }

        public void Debug(Address address, string message)
        {
            this.logger.LogDebug(message);

            var log = new Log
            {
                AddressId = address.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Debug"
            };

            this.dbContext.Add(log);
        }

        public void Debug(SslWildcard wildcard, string message)
        {
            this.logger.LogDebug(message);

            var log = new Log
            {
                SslWildcardId = wildcard.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Debug"
            };

            this.dbContext.Add(log);
        }

        public void Information(Backend backend, string message)
        {
            this.logger.LogInformation(message);

            var log = new Log
            {
                BackendId = backend.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Information"
            };

            this.dbContext.Add(log);
        }

        public void Warning(Backend backend, string message)
        {
            this.logger.LogWarning(message);

            var log = new Log
            {
                BackendId = backend.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Warning"
            };

            this.dbContext.Add(log);
        }

        public void Error(Backend backend, string message)
        {
            this.logger.LogError(message);

            var log = new Log
            {
                BackendId = backend.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Error"
            };

            this.dbContext.Add(log);
        }

        public void Debug(Backend backend, string message)
        {
            this.logger.LogDebug(message);

            var log = new Log
            {
                BackendId = backend.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Debug"
            };

            this.dbContext.Add(log);
        }

        public void Information(Frontend frontend, string message)
        {
            this.logger.LogInformation(message);

            var log = new Log
            {
                BackendId = frontend.Backend.Id,
                FrontendId = frontend.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Information"
            };

            this.dbContext.Add(log);
        }

        public void Warning(Frontend frontend, string message)
        {
            this.logger.LogWarning(message);

            var log = new Log
            {
                BackendId = frontend.Backend.Id,
                FrontendId = frontend.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Warning"
            };

            this.dbContext.Add(log);
        }

        public void Error(Frontend frontend, string message)
        {
            this.logger.LogError(message);

            var log = new Log
            {
                BackendId = frontend.Backend.Id,
                FrontendId = frontend.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Error"
            };

            this.dbContext.Add(log);
        }

        public void Debug(Frontend frontend, string message)
        {
            this.logger.LogDebug(message);

            var log = new Log
            {
                BackendId = frontend.Backend.Id,
                FrontendId = frontend.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Debug"
            };

            this.dbContext.Add(log);
        }

        public void Information(SubDomain subDomain, string message)
        {
            this.logger.LogInformation(message);

            var log = new Log
            {
                BackendId = subDomain.Frontend.Backend.Id,
                FrontendId = subDomain.Frontend.Id,
                SubDomainId = subDomain.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Information"
            };

            this.dbContext.Add(log);
        }

        public void Warning(SubDomain subDomain, string message)
        {
            this.logger.LogWarning(message);

            var log = new Log
            {
                BackendId = subDomain.Frontend.Backend.Id,
                FrontendId = subDomain.Frontend.Id,
                SubDomainId = subDomain.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Warning"
            };

            this.dbContext.Add(log);
        }

        public void Error(SubDomain subDomain, string message)
        {
            this.logger.LogError(message);

            var log = new Log
            {
                BackendId = subDomain.Frontend.Backend.Id,
                FrontendId = subDomain.Frontend.Id,
                SubDomainId = subDomain.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Error"
            };

            this.dbContext.Add(log);
        }

        public void Debug(SubDomain subDomain, string message)
        {
            this.logger.LogDebug(message);

            var log = new Log
            {
                BackendId = subDomain.Frontend.Backend.Id,
                FrontendId = subDomain.Frontend.Id,
                SubDomainId = subDomain.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Debug"
            };

            this.dbContext.Add(log);
        }

        public void Information(SslOrder sslOrder, string message)
        {
            this.logger.LogInformation(message);

            var log = new Log
            {
                BackendId = sslOrder.Frontend.Backend.Id,
                FrontendId = sslOrder.Frontend.Id,
                SslOrderId = sslOrder.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Information"
            };

            this.dbContext.Add(log);
        }

        public void Warning(SslOrder sslOrder, string message)
        {
            this.logger.LogWarning(message);

            var log = new Log
            {
                BackendId = sslOrder.Frontend.Backend.Id,
                FrontendId = sslOrder.Frontend.Id,
                SslOrderId = sslOrder.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Warning"
            };

            this.dbContext.Add(log);
        }

        public void Error(SslOrder sslOrder, string message)
        {
            this.logger.LogError(message);

            var log = new Log
            {
                BackendId = sslOrder.Frontend.Backend.Id,
                FrontendId = sslOrder.Frontend.Id,
                SslOrderId = sslOrder.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Error"
            };

            this.dbContext.Add(log);
        }

        public void Debug(SslOrder sslOrder, string message)
        {
            this.logger.LogDebug(message);

            var log = new Log
            {
                BackendId = sslOrder.Frontend.Backend.Id,
                FrontendId = sslOrder.Frontend.Id,
                SslOrderId = sslOrder.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Debug"
            };

            this.dbContext.Add(log);
        }

        public void Information(SslOrder sslOrder, SslAuthorization sslAuthorization, string message)
        {
            this.logger.LogInformation(message);

            var log = new Log
            {
                BackendId = sslOrder.Frontend.Backend.Id,
                FrontendId = sslOrder.Frontend.Id,
                SslOrderId = sslOrder.Id,
                SslAuthorizationId = sslAuthorization.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Information"
            };

            this.dbContext.Add(log);
        }

        public void Warning(SslOrder sslOrder, SslAuthorization sslAuthorization, string message)
        {
            this.logger.LogWarning(message);

            var log = new Log
            {
                BackendId = sslOrder.Frontend.Backend.Id,
                FrontendId = sslOrder.Frontend.Id,
                SslOrderId = sslOrder.Id,
                SslAuthorizationId = sslAuthorization.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Warning"
            };

            this.dbContext.Add(log);
        }

        public void Error(SslOrder sslOrder, SslAuthorization sslAuthorization, string message)
        {
            this.logger.LogError(message);

            var log = new Log
            {
                BackendId = sslOrder.Frontend.Backend.Id,
                FrontendId = sslOrder.Frontend.Id,
                SslOrderId = sslOrder.Id,
                SslAuthorizationId = sslAuthorization.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Error"
            };

            this.dbContext.Add(log);
        }

        public void Debug(SslOrder sslOrder, SslAuthorization sslAuthorization, string message)
        {
            this.logger.LogDebug(message);

            var log = new Log
            {
                BackendId = sslOrder.Frontend.Backend.Id,
                FrontendId = sslOrder.Frontend.Id,
                SslOrderId = sslOrder.Id,
                SslAuthorizationId = sslAuthorization.Id,
                Message = message,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published,
                Subject = "Debug"
            };

            this.dbContext.Add(log);
        }

        public async Task<List<FortiWebLog>> GetFortiWebLogsAsync(DateTime from, DateTime till, List<string> hosts)
        {
            return await this.dbContext.Set<FortiWebLog>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.DateTime > from)
                .Where(x => x.DateTime < till)
                .Where(x => !hosts.Any() || hosts.Contains(x.Host))
                .ToListAsync();
        }
    }
}