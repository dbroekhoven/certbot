using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPoolRule
    {
        [JsonPropertyName("results")]
        public FortiWebServerPoolRulesResult Result { get; set; }

        [JsonIgnore]
        public string Json { get; set; }

        [JsonIgnore]
        public bool HasError => this.Result.ErrorCode > 0;
    }
}
