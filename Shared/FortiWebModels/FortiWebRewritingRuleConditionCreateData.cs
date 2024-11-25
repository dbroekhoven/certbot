using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebRewritingRuleConditionCreateData
    {
        [JsonPropertyName("q_type")]
        public int QType { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string ObjectText { get; set; }

        [JsonPropertyName("object_val")]
        public string ObjectValue { get; set; }

        [JsonPropertyName("reg-exp")]
        public string RegExp { get; set; }

        [JsonPropertyName("is-essential_val")]
        public string IsEssentialVal { get; set; }

        [JsonPropertyName("reverse-match")]
        public string ReverseMatch { get; set; }

        [JsonPropertyName("reverse-match_val")]
        public string ReverseMatchVal { get; set; }

        [JsonPropertyName("protocol-filter")]
        public string ProtocolFilter { get; set; }

        [JsonPropertyName("protocol-filter_val")]
        public string ProtocolFilterVal { get; set; }

        [JsonPropertyName("HTTP-protocol_val")]
        public string HTTPProtocolVal { get; set; }

        [JsonPropertyName("content-type-filter_val")]
        public string ContentTypeFilterVal { get; set; }

        [JsonPropertyName("content-type-set_val")]
        public string ContentTypeSetVal { get; set; }

        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        [JsonIgnore]
        public string Json { get; set; }

        [JsonIgnore]
        public bool HasError => this.ErrorCode > 0;
    }
}
