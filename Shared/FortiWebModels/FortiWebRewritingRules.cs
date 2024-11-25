using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebRewritingRules
    {
        [JsonPropertyName("results")]
        public List<FortiWebRewritingRulesResult> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
