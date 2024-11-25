using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebRegistration
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
