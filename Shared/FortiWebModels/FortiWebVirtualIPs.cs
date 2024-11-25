using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebVirtualIPs
    {
        [JsonPropertyName("results")]
        public List<FortiWebVirtualIPsResult> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
