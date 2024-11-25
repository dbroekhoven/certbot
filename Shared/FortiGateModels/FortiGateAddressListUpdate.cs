using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiGateModels
{
    public class FortiGateAddressListUpdate
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("member")]
        public List<FortiGateAddressListMember> Members { get; set; }

        [JsonPropertyName("exclude")]
        public string Exclude { get; set; }

        [JsonPropertyName("color")]
        public string Color { get; set; }

        [JsonPropertyName("allow-routing")]
        public string AllowRouting { get; set; }

        [JsonPropertyName("fabric-object")]
        public string FabricObject { get; set; }
    }
}
