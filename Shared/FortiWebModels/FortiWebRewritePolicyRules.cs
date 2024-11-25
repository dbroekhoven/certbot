using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebRewritePolicyRules
    {
        [JsonPropertyName("results")]
        public List<FortiWebRewritePolicyRulesResult> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
