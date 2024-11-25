using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerNameIndicationsResult
    {
        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("seq")]
        public int Seq { get; set; }

        [JsonPropertyName("q_type")]
        public int QType { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("domain-type")]
        public string DomainType { get; set; }

        [JsonPropertyName("domain-type_val")]
        public string DomainTypeVal { get; set; }

        [JsonPropertyName("multi-local-cert")]
        public string MultiLocalCert { get; set; }

        [JsonPropertyName("multi-local-cert_val")]
        public string MultiLocalCertVal { get; set; }

        [JsonPropertyName("multi-local-cert-group")]
        public string MultiLocalCertGroup { get; set; }

        [JsonPropertyName("multi-local-cert-group_val")]
        public string MultiLocalCertGroupVal { get; set; }

        [JsonPropertyName("local-cert")]
        public string LocalCert { get; set; }

        [JsonPropertyName("local-cert_val")]
        public string LocalCertVal { get; set; }

        [JsonPropertyName("inter-group")]
        public string InterGroup { get; set; }

        [JsonPropertyName("inter-group_val")]
        public string InterGroupVal { get; set; }

        [JsonPropertyName("verify")]
        public string Verify { get; set; }

        [JsonPropertyName("verify_val")]
        public string VerifyVal { get; set; }
    }
}
