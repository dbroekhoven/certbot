using System.Text.Json.Serialization;

namespace Shared.FortiGateModels
{
    public class FortiGateAddressInterface
    {
        [JsonPropertyName("q_origin_key")]
        public string OriginKey { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("datasource")]
        public string Datasource { get; set; }
    }
}
