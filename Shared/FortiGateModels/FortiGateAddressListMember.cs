using System.Text.Json.Serialization;

namespace Shared.FortiGateModels
{
    public class FortiGateAddressListMember
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
