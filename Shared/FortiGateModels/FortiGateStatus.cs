using System.Text.Json.Serialization;

namespace Shared.FortiGateModels
{
    public class FortiGateStatus
    {
        [JsonPropertyName("version")]
        public string Version { get; set; }

        [JsonPropertyName("build")]
        public int Build { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("serial")]
        public string Serial { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
