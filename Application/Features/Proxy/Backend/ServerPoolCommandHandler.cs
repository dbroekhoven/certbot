using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Shared.Extensions;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class ServerPoolCommandHandler : IRequestHandler<ServerPoolCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly FortiWebService fortiWebService;

        public ServerPoolCommandHandler(DatabaseLogger logger, FortiWebService fortiWebService)
        {
            this.logger = logger;
            this.fortiWebService = fortiWebService;
        }

        public async Task<bool> Handle(ServerPoolCommand command, CancellationToken cancellationToken)
        {
            if (command.Backend.HasServerPool && !command.Backend.ChangedServerPool && !command.Backend.IsDeleted)
            {
                return true;
            }

            if (!command.Backend.HasServerPool && !command.Backend.ChangedServerPool && command.Backend.IsDeleted)
            {
                return true;
            }

            if (command.Backend.IsDeleted)
            {
                command.Backend.HasServerPool = false;
                command.Backend.HasServerPoolRule = false;

                return true;
            }

            if (command.Backend.ChangedServerPool)
            {
                return true;
                //return await this.DeleteObsoleteAsync(command);
            }

            return await this.CreateOrUpdateAsync(command);
        }

        public async Task<bool> CreateOrUpdateAsync(ServerPoolCommand command)
        {
            var serverPoolName = command.Backend.GetServerPoolName();
            var loadBalanced = command.Backend.IsLoadBalanced();

            this.logger.Information(command.Backend, $"Server pool {serverPoolName}");

            var allServerPools = await this.fortiWebService.GetServerPoolsAsync();
            if (allServerPools.Results.Any(x => x.Name == serverPoolName))
            {
                this.logger.Warning(command.Backend, $"Server pool {serverPoolName} already exists");

                command.Backend.HasServerPool = true;

                return true;
            }

            var result = await this.fortiWebService.CreateServerPoolAsync(serverPoolName, loadBalanced);
            this.logger.Debug(command.Backend, $"CreateServerPoolAsync: {result.Json}");

            if (result.HasError)
            {
                this.logger.Error(command.Backend, $"Server pool failed: {serverPoolName}, Errorcode {result.Result.ErrorCode}");

                return false;
            }

            this.logger.Information(command.Backend, $"Server pool {serverPoolName} created");

            command.Backend.HasServerPool = true;

            return true;
        }

        //public async Task<bool> DeleteAsync(ServerPoolCommand command)
        //{
        //    var serverPoolName = command.Backend.GetServerPoolName();

        //    this.logger.Information(command.Backend, $"Server pool {serverPoolName}");

        //    var allServerPools = await this.fortiWebService.GetServerPoolsAsync();
        //    if (!allServerPools.Results.Any(x => x.Name == serverPoolName))
        //    {
        //        this.logger.Warning(command.Backend, $"Server pool {serverPoolName} does not exists");

        //        command.Backend.Status = Cre8ion.Database.EntityStatus.Archive;

        //        return false;
        //    }

        //    var result = await this.fortiWebService.DeleteServerPoolAsync(serverPoolName);
        //    this.logger.Debug(command.Backend, $"DeleteServerPoolAsync: {result.Json}");

        //    if (result.HasError)
        //    {
        //        this.logger.Error(command.Backend, $"Server pool failed: {serverPoolName}, Errorcode {result.Result.ErrorCode}");

        //        return false;
        //    }

        //    this.logger.Information(command.Backend, $"Server pool {serverPoolName} deleted");

        //    command.Backend.Status = Cre8ion.Database.EntityStatus.Archive;

        //    return true;
        //}

        //public async Task<bool> DeleteObsoleteAsync(ServerPoolCommand command)
        //{
        //    var serverPoolName = command.Backend.GetObsoleteServerPoolName();

        //    this.logger.Information(command.Backend, $"Server pool {serverPoolName}");

        //    var allServerPools = await this.fortiWebService.GetServerPoolsAsync();
        //    if (!allServerPools.Results.Any(x => x.Name == serverPoolName))
        //    {
        //        this.logger.Warning(command.Backend, $"Server pool {serverPoolName} does not exists");

        //        command.Backend.ChangedServerPool = false;

        //        return false;
        //    }

        //    var result = await this.fortiWebService.DeleteServerPoolAsync(serverPoolName);
        //    this.logger.Debug(command.Backend, $"DeleteServerPoolAsync: {result.Json}");

        //    if (result.HasError)
        //    {
        //        this.logger.Error(command.Backend, $"Server pool failed: {serverPoolName}, Errorcode {result.Result.ErrorCode}");

        //        return false;
        //    }

        //    this.logger.Information(command.Backend, $"Server pool {serverPoolName} deleted");

        //    command.Backend.ChangedServerPool = false;

        //    return true;
        //}
    }
}