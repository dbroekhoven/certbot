using System.Text.Json.Serialization;

namespace Shared.FortiGateModels
{
    public class FortiGateAddressCreateInterface
    {
        [JsonPropertyName("q_origin_key")]
        public string Port { get; set; }
    }
}
