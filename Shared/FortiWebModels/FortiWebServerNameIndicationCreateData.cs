using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerNameIndicationCreateData
    {
        [JsonPropertyName("domain-type")]
        public string DomainType { get; set; }

        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("multi-local-cert")]
        public string MultiLocalCert { get; set; }

        [JsonPropertyName("multi-local-cert-group")]
        public string MultiLocalCertGroup { get; set; }

        [JsonPropertyName("local-cert")]
        public string LocalCert { get; set; }

        [JsonPropertyName("inter-group")]
        public string IntermediateGroup { get; set; }

        [JsonPropertyName("verify")]
        public string Verify { get; set; }
    }
}
