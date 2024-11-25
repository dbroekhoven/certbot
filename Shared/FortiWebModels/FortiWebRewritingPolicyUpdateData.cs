using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebRewritingPolicyUpdateData
    {
        [JsonPropertyName("q_type")]
        public int QType { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("url-rewrite-rule-name")]
        public string UrlRewriteRuleName { get; set; }

        [JsonPropertyName("url-rewrite-rule-name_val")]
        public string UrlRewriteRuleNameVal { get; set; }

        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        [JsonIgnore]
        public string Json { get; set; }

        [JsonIgnore]
        public bool HasError => this.ErrorCode > 0;
    }
}
