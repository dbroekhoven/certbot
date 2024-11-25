using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebVirtualServerAddressData
    {
        [JsonPropertyName("data")]
        public FortiWebVirtualServerAddressResult Data { get; set; }
    }
}
