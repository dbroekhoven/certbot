using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebVirtualServerAddressResult
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("use-interface-ip")]
        public string UseInterfaceIP { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("vip")]
        public string Vip { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
