using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPoolRuleCreateData
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("server-type")]
        public string Type { get; set; }

        [JsonPropertyName("ip")]
        public string Address { get; set; }

        [JsonPropertyName("http2")]
        public string Http2 { get; set; }

        [JsonPropertyName("health-check-inherit")]
        public string HealthCheckInherit { get; set; }
    }
}
