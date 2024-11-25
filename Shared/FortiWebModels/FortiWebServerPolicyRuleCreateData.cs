using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPolicyRuleCreateData
    {
        [JsonPropertyName("profile-inherit")]
        public string ProfileInherit { get; set; }

        [JsonPropertyName("is-default")]
        public string IsDefault { get; set; }

        [JsonPropertyName("content-routing-policy-name")]
        public string ContentRoutingPolicyName { get; set; }

        [JsonPropertyName("web-protection-profile")]
        public string WebProtectionProfile { get; set; }
    }
}
