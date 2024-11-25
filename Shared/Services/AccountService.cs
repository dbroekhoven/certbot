using System;
using System.Linq;
using System.Threading.Tasks;
using Certes;
using Cre8ion.Common;
using Cre8ion.Database;
using Cre8ion.Database.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Shared.DatabaseEntities;

namespace Shared.Services
{
    public class AccountService : IService
    {
        private readonly DatabaseContext dbContext;
        private readonly HttpService httpService;

        public AccountService(DatabaseContext dbContext, HttpService httpService)
        {
            this.dbContext = dbContext;
            this.httpService = httpService;
        }

        public async Task<(bool Online, string Message, string Json)> GetAcmeStatusAsync()
        {
            var directory = Certes.Acme.WellKnownServers.LetsEncryptV2;

            var url = directory.ToString().Replace("/directory", string.Empty);

            var result = await this.httpService.GetAsync(url);
            var online = !result.Contains("error", StringComparison.InvariantCultureIgnoreCase);
            var message = online ? "Online" : "Error";

            return (online, message, result);
        }

        public async Task<string> GetAccountKeyAsync()
        {
            var account = await this.GetOrCreateAccountAsync();

            return account.Key;
        }

        public async Task<Account> GetOrCreateAccountAsync()
        {
            var account = await this.GetAccountAsync();
            if (account == null)
            {
            }

            return account;
        }

        public async Task<Account> GetAccountAsync()
        {
            var result = await this.dbContext
                .Set<Account>()
                .Where(x => x.Status == EntityStatus.Published)
                .Where(x => x.Email == Settings.LetsEncryptEmailAddress)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<Account> CreateAccountAsync()
        {
            var directory = Certes.Acme.WellKnownServers.LetsEncryptV2;

            var acme = new AcmeContext(directory);

            await acme.NewAccount(Settings.LetsEncryptEmailAddress, true);

            var key = acme.AccountKey.ToPem();

            var account = new Account
            {
                Email = Settings.LetsEncryptEmailAddress,
                Key = key,
                DateTime = DateTime.Now,
                Status = EntityStatus.Published
            };

            this.dbContext
                .Add(account);

            await this.dbContext
                .SaveChangesAsync();

            return account;
        }

        public IAcmeContext GetAcmeContext(string pem)
        {
            var key = KeyFactory.FromPem(pem);

            var directory = Certes.Acme.WellKnownServers.LetsEncryptV2;

            return new AcmeContext(directory, key);
        }
    }
}