using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPoolRuleCreate
    {
        [JsonPropertyName("data")]
        public FortiWebServerPoolRuleCreateData Data { get; set; }
    }
}
