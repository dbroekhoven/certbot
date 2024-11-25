using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebContentRoutingRules
    {
        [JsonPropertyName("results")]
        public List<FortiWebContentRoutingRulesResult> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
