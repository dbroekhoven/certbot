using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Services;

namespace Application.Features.LetsEncrypt
{
    public class NewOrdersCommandHandler : IRequestHandler<NewOrdersCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly AccountService accountService;
        private readonly LetsEncryptService letsEncryptService;

        public NewOrdersCommandHandler(DatabaseLogger logger, AccountService accountService, LetsEncryptService letsEncryptService)
        {
            this.logger = logger;
            this.accountService = accountService;
            this.letsEncryptService = letsEncryptService;
        }

        public async Task<bool> Handle(NewOrdersCommand command, CancellationToken cancellationToken)
        {
            this.logger.Information(command.Frontend, $"New order or renewal for: {command.Frontend.Name}");

            var key = await this.accountService.GetAccountKeyAsync();
            var acme = this.accountService.GetAcmeContext(key);

            var order = await this.letsEncryptService.NewOrderAsync(acme, command.Frontend);

            if (order == null)
            {
                this.logger.Warning(command.Frontend, $"Failed to create new order for domain {command.Frontend.Name}.");

                return false;
            }

            if (command.Frontend.Reissue)
            {
                command.Frontend.ReissueCount++;
                command.Frontend.Reissue = false;
            }

            this.logger.Information(order, $"Order created for: {command.Frontend.Name}");

            return true;
        }
    }
}