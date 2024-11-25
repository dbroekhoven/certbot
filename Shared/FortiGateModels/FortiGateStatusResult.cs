using System.Text.Json.Serialization;

namespace Shared.FortiGateModels
{
    public class FortiGateStatusResult
    {
        [JsonPropertyName("hostName")]
        public string HostName { get; set; }
    }
}
