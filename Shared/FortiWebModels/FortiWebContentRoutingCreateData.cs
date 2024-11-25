using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebContentRoutingCreateData
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("server-pool")]
        public string ServerPool { get; set; }
    }
}
