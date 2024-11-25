using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPolicyRuleCreate
    {
        [JsonPropertyName("data")]
        public FortiWebServerPolicyRuleCreateData Data { get; set; }
    }
}
