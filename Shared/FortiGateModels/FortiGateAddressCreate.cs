using System.Text.Json.Serialization;

namespace Shared.FortiGateModels
{
    public class FortiGateAddressCreate
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("subnet")]
        public string Subnet { get; set; }

        [JsonPropertyName("ip6")]
        public string Address6 { get; set; }

        [JsonPropertyName("associated-interface")]
        public FortiGateAddressCreateInterface Interface { get; set; }
    }
}
