using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Services;

namespace Application.Features.LetsEncrypt
{
    public class TestOrdersCommandHandler : IRequestHandler<TestOrdersCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly LetsEncryptService letsEncryptService;

        public TestOrdersCommandHandler(DatabaseLogger logger, LetsEncryptService letsEncryptService)
        {
            this.logger = logger;
            this.letsEncryptService = letsEncryptService;
        }

        public async Task<bool> Handle(TestOrdersCommand command, CancellationToken cancellationToken)
        {
            this.logger.Information(command.SslOrder.Frontend, $"Test domain: {command.SslOrder.Frontend.Name}");

            this.logger.Information(command.SslOrder, $"Test order for: {command.SslOrder.Frontend.Name}");

            var result = await this.letsEncryptService.TestAsync(command.SslOrder);
            if (result)
            {
                command.SslOrder.IsTested = true;
            }

            return true;
        }
    }
}