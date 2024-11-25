using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPools
    {
        [JsonPropertyName("results")]
        public List<FortiWebServerPoolResult> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
