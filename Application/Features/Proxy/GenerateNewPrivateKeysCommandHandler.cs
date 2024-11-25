using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Certes;
using Cre8ion.Database;
using Cre8ion.Database.EntityFramework;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.DatabaseEntities;
using Shared.Services;

namespace Application.Features.Proxy
{
    public class GenerateNewPrivateKeysCommandHandler : IRequestHandler<GenerateNewPrivateKeysCommand, bool>
    {
        private readonly DatabaseLogger logger;
        private readonly DatabaseContext dbContext;

        public GenerateNewPrivateKeysCommandHandler(DatabaseLogger logger, DatabaseContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public async Task<bool> Handle(GenerateNewPrivateKeysCommand command, CancellationToken cancellationToken)
        {
            var processed = await this.dbContext.Set<SslPrivateKey>()
                .IgnoreQueryFilters()
                .Where(x => x.Status == EntityStatus.Archive)
                .ToListAsync();

            this.logger.Information($"Total processed private keys: {processed.Count}");

            if (processed.Any())
            {
                foreach (var key in processed)
                {
                    this.dbContext.Remove(key);
                }
            }

            var unused = await this.dbContext.Set<SslPrivateKey>()
                .IgnoreQueryFilters()
                .Where(x => x.Status == EntityStatus.Published)
                .ToListAsync();

            this.logger.Information($"Total unused private keys: {unused.Count}");

            for (var i = 1; i <= 10; i++)
            {
                this.logger.Information($"Generate private key with keysize 4096");

                var privateKey = KeyFactory.NewKey(KeyAlgorithm.RS256, 4096);

                this.logger.Information($"Generate pem from private key");

                var key = new SslPrivateKey
                {
                    Status = EntityStatus.Published,
                    DateTime = DateTime.Now,
                    Value = privateKey.ToPem()
                };

                this.dbContext.Add(key);
            }

            await this.dbContext.SaveChangesAsync();

            return true;
        }
    }
}