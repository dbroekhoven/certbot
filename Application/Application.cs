using System;
using System.Threading.Tasks;
using Application.Infrastructure;
using Cre8ion.Common;
using Cre8ion.Features.Synchronization;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Application
{
    public class Application : IService
    {
        private readonly SyncRunner runner;
        private readonly CommandLineOptions options;
        private readonly IHostEnvironment environment;
        private readonly IServiceProvider provider;
        private readonly ILogger<Application> logger;

        public Application(
            SyncRunner runner,
            CommandLineOptions options,
            IHostEnvironment environment,
            IServiceProvider provider,
            ILogger<Application> logger)
        {
            this.runner = runner;
            this.options = options;
            this.environment = environment;
            this.provider = provider;
            this.logger = logger;
        }

        public async Task RunApplicationAsync()
        {
            if (this.options.Scheduled)
            {
                await this.CheckPendingRequestsAsync();
            }
            else
            {
                await this.RunSynchronizersAsync(this.options);
            }
        }

        private async Task CheckPendingRequestsAsync()
        {
            var requests = await this.runner.GetRequestsAsync();

            foreach (var request in requests)
            {
                var isRunning = await this.runner.IsRunningAsync(request.TypeId);
                if (isRunning)
                {
                    continue;
                }

                var synchronizer = await this.runner.GetSynchronizerAsync(this.options, request.TypeId);
                if (synchronizer == null)
                {
                    continue;
                }

                await this.RunAsync(synchronizer.Name, synchronizer.Id);

                await this.runner.UpdateRequestAsync(request);
            }
        }
        private async Task RunSynchronizersAsync(CommandLineOptions options)
        {
            var synchronizers = await this.runner.GetSynchronizersAsync(options);

            foreach (var synchronizer in synchronizers)
            {
                await this.RunAsync(synchronizer.Name, synchronizer.Id);
            }
        }

        private async Task RunAsync(Type type, int number)
        {
            this.logger.LogInformation($">>>> {type.Name} started");

            var result = await this.runner.RunAsync(number, async (DateTime? date) =>
            {
                var syncer = (ISynchronizer)this.provider.GetService(type);

                return await syncer.SynchronizeAsync(date);
            });

            if (!result.Succeeded)
            {
                this.logger.LogError(result.Message);
            }

            this.logger.LogInformation($">>>> {type.Name} finished");
        }
    }
}