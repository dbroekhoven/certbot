using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebVirtualIPsData
    {
        [JsonPropertyName("data")]
        public FortiWebVirtualIPsResult Data { get; set; }
    }
}
