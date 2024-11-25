using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebRewritingRuleConditionCreate
    {
        [JsonPropertyName("data")]
        public FortiWebRewritingRuleConditionCreateData Data { get; set; }
    }
}
