using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiGateModels
{
    public class FortiGateAddressList
    {
        [JsonPropertyName("results")]
        public List<FortiGateAddressListResult> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
