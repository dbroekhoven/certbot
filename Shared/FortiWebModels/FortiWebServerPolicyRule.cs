using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPolicyRule
    {
        [JsonPropertyName("results")]
        public FortiWebServerPolicyRulesResult Result { get; set; }

        [JsonIgnore]
        public string Json { get; set; }

        [JsonIgnore]
        public bool HasError => this.Result.ErrorCode > 0;
    }
}
