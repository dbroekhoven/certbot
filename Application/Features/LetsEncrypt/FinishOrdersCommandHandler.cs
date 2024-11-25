using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Services;

namespace Application.Features.LetsEncrypt
{
    public class FinishOrdersCommandHandler : IRequestHandler<FinishOrdersCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly AccountService accountService;
        private readonly LetsEncryptService letsEncryptService;
        private readonly DomainService domainService;

        public FinishOrdersCommandHandler(DatabaseLogger logger, AccountService accountService, LetsEncryptService letsEncryptService, DomainService domainService)
        {
            this.logger = logger;
            this.accountService = accountService;
            this.letsEncryptService = letsEncryptService;
            this.domainService = domainService;
        }

        public async Task<bool> Handle(FinishOrdersCommand command, CancellationToken cancellationToken)
        {
            var key = await this.accountService.GetAccountKeyAsync();
            var acme = this.accountService.GetAcmeContext(key);

            this.logger.Information(command.SslOrder, $"Finish domain: {command.SslOrder.Frontend.Name}");

            var data = await this.domainService.GetOrderDataAsync(command.SslOrder.Id);

            await this.letsEncryptService.FinishAsync(acme, command.SslOrder, data);

            return true;
        }
    }
}