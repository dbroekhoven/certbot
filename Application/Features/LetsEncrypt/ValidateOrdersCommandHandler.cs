using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Services;

namespace Application.Features.LetsEncrypt
{
    public class ValidateOrdersCommandHandler : IRequestHandler<ValidateOrdersCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly AccountService accountService;
        private readonly LetsEncryptService letsEncryptService;

        public ValidateOrdersCommandHandler(DatabaseLogger logger, AccountService accountService, LetsEncryptService letsEncryptService)
        {
            this.logger = logger;
            this.accountService = accountService;
            this.letsEncryptService = letsEncryptService;
        }

        public async Task<bool> Handle(ValidateOrdersCommand command, CancellationToken cancellationToken)
        {
            var key = await this.accountService.GetAccountKeyAsync();
            var acme = this.accountService.GetAcmeContext(key);

            this.logger.Information(command.SslOrder, $"Validate domain: {command.SslOrder.Frontend.Name}");

            await this.letsEncryptService.ValidateAsync(acme, command.SslOrder);

            return true;
        }
    }
}