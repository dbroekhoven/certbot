using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebVirtualServerAddress
    {
        [JsonPropertyName("results")]
        public List<FortiWebVirtualServerAddressResult> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
