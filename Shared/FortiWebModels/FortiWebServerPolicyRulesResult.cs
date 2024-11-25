using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPolicyRulesResult
    {
        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("seq")]
        public int Seq { get; set; }

        [JsonPropertyName("q_type")]
        public int QType { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("content-routing-policy-name")]
        public string ContentRoutingPolicyName { get; set; }

        [JsonPropertyName("content-routing-policy-name_val")]
        public string ContentRoutingPolicyNameVal { get; set; }

        [JsonPropertyName("profile-inherit")]
        public string ProfileInherit { get; set; }

        [JsonPropertyName("profile-inherit_val")]
        public string ProfileInheritVal { get; set; }

        [JsonPropertyName("web-protection-profile")]
        public string WebProtectionProfile { get; set; }

        [JsonPropertyName("web-protection-profile_val")]
        public string WebProtectionProfileVal { get; set; }

        [JsonPropertyName("is-default")]
        public string IsDefault { get; set; }

        [JsonPropertyName("is-default_val")]
        public string IsDefaultVal { get; set; }
    }
}
