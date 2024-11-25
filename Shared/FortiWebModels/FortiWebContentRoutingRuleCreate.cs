using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebContentRoutingRuleCreate
    {
        [JsonPropertyName("data")]
        public FortiWebContentRoutingRuleCreateData Data { get; set; }
    }
}
