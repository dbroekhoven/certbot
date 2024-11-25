using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebVirtualIPsResult
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("vip")]
        public string Vip4 { get; set; }

        [JsonPropertyName("vip6")]
        public string Vip6 { get; set; }

        [JsonPropertyName("interface")]
        public string Interface { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
