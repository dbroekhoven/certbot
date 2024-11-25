using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebRewritingRuleCreate
    {
        [JsonPropertyName("data")]
        public FortiWebRewritingRuleCreateData Data { get; set; }
    }
}
