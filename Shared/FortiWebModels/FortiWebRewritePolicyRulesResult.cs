using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebRewritePolicyRulesResult
    {
        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("url-rewrite-rule-name")]
        public string Name { get; set; }
    }
}
