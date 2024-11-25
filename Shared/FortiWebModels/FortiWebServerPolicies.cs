using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPolicies
    {
        [JsonPropertyName("results")]
        public List<FortiWebServerPoliciesResult> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
