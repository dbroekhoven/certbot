using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebContentRoutingRuleCreateData
    {
        [JsonPropertyName("match-object")]
        public string MatchObject { get; set; }

        [JsonPropertyName("match-condition")]
        public string MatchCondition { get; set; }

        [JsonPropertyName("match-expression")]
        public string MatchExpression { get; set; }

        [JsonPropertyName("concatenate")]
        public string Concatenate { get; set; }
    }
}
