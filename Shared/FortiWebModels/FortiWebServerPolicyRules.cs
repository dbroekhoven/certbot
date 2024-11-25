using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPolicyRules
    {
        [JsonPropertyName("results")]
        public List<FortiWebServerPolicyRulesResult> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
