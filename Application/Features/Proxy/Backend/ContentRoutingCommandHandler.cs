using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Extensions;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class ContentRoutingCommandHandler : IRequestHandler<ContentRoutingCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiWebService fortiWebService;

        public ContentRoutingCommandHandler(DatabaseLogger logger, FortiWebService fortiWebService)
        {
            this.logger = logger;
            this.fortiWebService = fortiWebService;
        }

        public async Task<bool> Handle(ContentRoutingCommand command, CancellationToken cancellationToken)
        {
            if (command.Backend.HasContentRouting && !command.Backend.ChangedServerPool && !command.Backend.IsDeleted)
            {
                return true;
            }

            if (!command.Backend.HasContentRouting && !command.Backend.ChangedServerPool && command.Backend.IsDeleted)
            {
                return true;
            }

            if (command.Backend.IsDeleted)
            {
                return await this.DeleteAsync(command);
            }

            if (command.Backend.ChangedServerPool)
            {
                await this.UpdateAsync(command);

                return true;
            }

            return await this.CreateAsync(command);
        }

        public async Task<bool> CreateAsync(ContentRoutingCommand command)
        {
            var serverPoolName = command.Backend.GetServerPoolName();
            var contentRoutingName = command.Backend.GetContentRoutingName();

            var contentRoutings = await this.fortiWebService.GetContentRoutingsAsync();
            if (contentRoutings.Results.Any(x => x.Name == contentRoutingName))
            {
                this.logger.Warning(command.Backend, $"CreateAsync: Content routing failed: {contentRoutingName} already exists");

                command.Backend.HasContentRouting = true;

                return true;
            }

            var result = await this.fortiWebService.CreateContentRoutingAsync(serverPoolName, contentRoutingName);
            this.logger.Debug(command.Backend, $"CreateContentRoutingAsync: {result.Json}");

            if (result.HasError)
            {
                this.logger.Error(command.Backend, $"CreateAsync: Content routing failed: {contentRoutingName}, Errorcode {result.Result.ErrorCode}");

                return false;
            }

            this.logger.Information(command.Backend, $"CreateAsync: Content routing created: {contentRoutingName}");

            command.Backend.HasContentRouting = true;

            return true;
        }

        public async Task<bool> UpdateAsync(ContentRoutingCommand command)
        {
            var serverPoolName = command.Backend.GetServerPoolName();
            var contentRoutingName = command.Backend.GetContentRoutingName();

            var contentRoutings = await this.fortiWebService.GetContentRoutingsAsync();
            if (!contentRoutings.Results.Any(x => x.Name == contentRoutingName))
            {
                this.logger.Warning(command.Backend, $"UpdateAsync: Content routing failed: {contentRoutingName} does not exists");

                return true;
            }

            var result = await this.fortiWebService.UpdateContentRoutingAsync(serverPoolName, contentRoutingName);
            this.logger.Debug(command.Backend, $"UpdateContentRoutingAsync: {result.Json}");

            if (result.HasError)
            {
                this.logger.Error(command.Backend, $"UpdateAsync: Content routing failed: {contentRoutingName}, Errorcode {result.Result.ErrorCode}");

                return false;
            }

            this.logger.Information(command.Backend, $"UpdateAsync: Content routing updated: {contentRoutingName}");

            command.Backend.HasContentRouting = true;
            command.Backend.ChangedServerPool = false;

            return true;
        }

        public async Task<bool> DeleteAsync(ContentRoutingCommand command)
        {
            var serverPoolName = command.Backend.GetServerPoolName();
            var contentRoutingName = command.Backend.GetContentRoutingName();

            var contentRoutings = await this.fortiWebService.GetContentRoutingsAsync();
            if (!contentRoutings.Results.Any(x => x.Name == contentRoutingName))
            {
                this.logger.Warning(command.Backend, $"DeleteAsync: Content routing failed: {contentRoutingName} does not exists");

                command.Backend.HasContentRouting = false;

                return true;
            }

            var rule = contentRoutings.Results.Find(x => x.Name == contentRoutingName);
            var result = await this.fortiWebService.DeleteContentRoutingAsync(contentRoutingName);
            this.logger.Debug(command.Backend, $"DeleteContentRoutingAsync: {result.Json}");

            if (result.HasError)
            {
                this.logger.Error(command.Backend, $"DeleteAsync: Content routing failed: {contentRoutingName}, Errorcode {result.Result.ErrorCode}");

                return false;
            }

            this.logger.Information(command.Backend, $"DeleteAsync: Content routing deleted: {contentRoutingName}");

            command.Backend.HasContentRouting = false;

            return true;
        }
    }
}