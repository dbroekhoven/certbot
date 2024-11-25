using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPoolCreateData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("health")]
        public string Health { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("server-balance")]
        public string ServerBalance { get; set; }

        [JsonPropertyName("lb-algo")]
        public string LoadBalanceMethod { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("http-reuse")]
        public string HttpReuse { get; set; }

        [JsonPropertyName("reuse-conn-max-request")]
        public int ReuseConnMaxRequest { get; set; }

        [JsonPropertyName("reuse-conn-max-count")]
        public int ReuseConnMaxCount { get; set; }
    }
}
