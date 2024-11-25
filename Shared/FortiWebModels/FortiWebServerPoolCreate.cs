using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPoolCreate
    {
        [JsonPropertyName("data")]
        public FortiWebServerPoolCreateData Data { get; set; }
    }
}
