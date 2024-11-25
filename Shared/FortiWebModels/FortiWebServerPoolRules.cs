using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPoolRules
    {
        [JsonPropertyName("results")]
        public List<FortiWebServerPoolRulesResult> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
