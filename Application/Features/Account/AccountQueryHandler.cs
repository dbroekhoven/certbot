using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Services;

namespace Application.Features.Account
{
    public class AccountQueryHandler : IRequestHandler<AccountQuery, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly AccountService accountService;

        public AccountQueryHandler(DatabaseLogger logger, AccountService accountService)
        {
            this.logger = logger;
            this.accountService = accountService;
        }

        public async Task<bool> Handle(AccountQuery query, CancellationToken cancellationToken)
        {
            var valid = true;

            this.logger.Information($"Checking Let's Encrypt server and account status");

            var productionStatus = await this.accountService.GetAcmeStatusAsync();

            if (!productionStatus.Online)
            {
                this.logger.Error($"Production server status is {productionStatus.Message}");

                valid = false;
            }
            else
            {
                this.logger.Information($"Production server status is {productionStatus.Message}");

                var productionAccountStatus = await this.GetAccountStatusAsync();
                this.logger.Information($"Production account status is {productionAccountStatus}");

                if (productionAccountStatus != "Valid")
                {
                    valid = false;
                }
            }

            return valid;
        }

        public async Task<string> GetAccountStatusAsync()
        {
            var key = await this.accountService.GetAccountKeyAsync();
            var acme = this.accountService.GetAcmeContext(key);
            var account = await acme.Account();
            var resource = await account.Resource();
            var accountStatus = resource.Status.ToString();

            return accountStatus;
        }
    }
}