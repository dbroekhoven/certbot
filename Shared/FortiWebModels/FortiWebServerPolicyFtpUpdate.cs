using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPolicyFtpUpdate
    {
        [JsonPropertyName("data")]
        public FortiWebServerPolicyFtpUpdateData Data { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
