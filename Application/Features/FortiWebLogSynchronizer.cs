using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Application.Infrastructure;
using Cre8ion.Common;
using Cre8ion.Common.Extensions;
using Cre8ion.Database;
using Cre8ion.Database.EntityFramework;
using Cre8ion.Features.Synchronization;
using MediatR;
using Shared.Services;

namespace Application.Features
{
    public class FortiWebLogSynchronizer : ISynchronizer, IService
    {
        private readonly CommandLineOptions options;
        private readonly DatabaseLogger logger;
        private readonly IMediator mediator;
        private readonly DatabaseContext dbContext;
        private readonly FortiWebLogService fortiWebLogService;

        public FortiWebLogSynchronizer(CommandLineOptions options, DatabaseLogger logger, IMediator mediator, DatabaseContext dbContext, FortiWebLogService fortiWebLogService)
        {
            this.options = options;
            this.logger = logger;
            this.mediator = mediator;
            this.dbContext = dbContext;
            this.fortiWebLogService = fortiWebLogService;
        }

        public async Task<string> SynchronizeAsync(DateTime? lastSyncDate)
        {
            var from = lastSyncDate ?? DateTime.Today;
            var till = DateTime.Today.AddDays(1);
            var destinations = new List<string>();

            if (!string.IsNullOrEmpty(this.options.From))
            {
                from = DateTime.ParseExact(this.options.From, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(this.options.Till))
            {
                till = DateTime.ParseExact(this.options.Till, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            }

            this.logger.Information($"Log range: {from:dd-MM-yyyy} - {from:HH:mm:ss} till {till:dd-MM-yyyy} - {till:HH:mm:ss}");

            List<Shared.FortiWebModels.FortiWebLogItem> fortiWebLogs = null;

            try
            {
                fortiWebLogs = await this.fortiWebLogService.GetLogAsync(Shared.FortiWebModels.FortiWebLogType.Attack, from, till, destinations);
            }
            catch (Exception exception)
            {
                this.logger.Information($"Failed to download logs from FortiWeb: {exception.Message}");
            }

            if (fortiWebLogs != null)
            {
                this.logger.Information($"Save {fortiWebLogs.Count} logs into database.");

                foreach (var fortiWebLog in fortiWebLogs)
                {
                    var dbLog = new Shared.DatabaseEntities.FortiWebLog
                    {
                        Date = DateTime.ParseExact(fortiWebLog.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        DateTime = DateTime.ParseExact($"{fortiWebLog.Date} {fortiWebLog.Time}", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                        Policy = fortiWebLog.Policy,
                        Country = fortiWebLog.Srccountry,
                        Destination = fortiWebLog.Dst,
                        ServerPool = fortiWebLog.ServerPoolName,
                        HttpPolicy = fortiWebLog.HttpPolicy,
                        SignaturePolicy = fortiWebLog.SignaturePolicy,
                        Source = fortiWebLog.Src,
                        Host = fortiWebLog.HttpHost,
                        MainType = fortiWebLog.MainType,
                        SubType = fortiWebLog.SubType,
                        Service = fortiWebLog.Service,
                        Url = fortiWebLog.HttpUrl.MaxLength(480, true),
                        UserAgent = fortiWebLog.HttpAgent.MaxLength(480, true),
                        Severity = fortiWebLog.SeverityLevel,
                        Action = fortiWebLog.Action,
                        Status = EntityStatus.Published
                    };

                    this.dbContext.Add(dbLog);
                }
            }

            await this.dbContext.SaveChangesAsync();

            return "Done!";
        }
    }
}