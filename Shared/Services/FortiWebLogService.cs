using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Cre8ion.Common;
using Shared.FortiWebModels;

namespace Shared.Services
{
    public class FortiWebLogService : IService
    {
        private readonly HttpService httpService;

        public FortiWebLogService(HttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<List<FortiWebLogItem>> GetLogAsync(FortiWebLogType type, DateTime from, DateTime till, List<string> destinations)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/log/logaccess.{type.ToString().ToLowerInvariant()}";

            var saving = TimeZoneInfo.Local.IsDaylightSavingTime(from);
            var offset = (saving ? -1 : -0) - TimeZoneInfo.Local.BaseUtcOffset.TotalHours;

            var utcDateFrom = from.AddHours(offset);
            var utcDateTill = till.AddHours(offset);

            var dateFilter = new FortiWebLogFilter
            {
                Id = "rel_time",
                Logic = new FortiWebLogFilterLogic
                {
                    Is = new FortiWebLogFilterComparison
                    {
                        IsTimestamp = true,
                    },
                    Search = "string",
                    Splitter = "-",
                    Range = 1
                },
                Value = new List<string>
                {
                    utcDateFrom.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString(),
                    utcDateTill.Subtract(new DateTime(1970, 1, 1)).TotalSeconds.ToString()
                }
            };

            var dstFilter = new FortiWebLogFilter
            {
                Id = "dst",
                Logic = new FortiWebLogFilterLogic
                {
                    Is = new FortiWebLogFilterComparison
                    {
                        IsString = true,
                    },
                    Search = "string",
                    Splitter = ","
                },
                Value = destinations
            };

            var levelFilter = new FortiWebLogFilter
            {
                Id = "severity_level",
                Logic = new FortiWebLogFilterLogic
                {
                    Is = new FortiWebLogFilterComparison
                    {
                        IsEnum = true,
                    },
                    Search = "string",
                    Modifiers = new List<string>
                    {
                        "!"
                    },
                    Not = 1
                },
                Value = new List<string>
                {
                    "Informative"
                }
            };

            var filters = new FortiWebLogFilter[]
            {
                dateFilter,
                dstFilter,
                levelFilter
            };

            var jsonOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var filterJson = JsonSerializer.Serialize(filters, jsonOptions);

            var payload = new List<FortiWebLogItem>();
            var totalCount = 0;
            var currentPage = 1;
            var filterOffset = 0;

            do
            {
                var data = new Dictionary<string, string>
                {
                    ["page"] = currentPage.ToString(),
                    ["filter"] = filterJson,
                    ["filter_offset"] = filterOffset.ToString(),
                    ["log"] = "alog.log",
                    ["size"] = "100"
                };

                var queryString = string.Join("&", data.Select(kvp => $"{kvp.Key}={kvp.Value}"));

                var result = await this.httpService.GetJsonAsync<FortiWebLog>($"{url}?{queryString}", HttpCredentials.WithAuthorization(Settings.FortiWebToken));

                filterOffset = result.Results.FilterOffset;
                totalCount = result.Results.Total;
                currentPage++;

                payload.AddRange(result.Results.Payload);

            } while (payload.Count < totalCount);

            return payload;
        }
    }
}