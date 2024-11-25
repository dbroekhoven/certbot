using System.Text.Json.Serialization;

namespace Shared.FortiGateModels
{
    public class FortiGatePostResult
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
