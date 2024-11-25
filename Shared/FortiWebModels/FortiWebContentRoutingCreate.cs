using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebContentRoutingCreate
    {
        [JsonPropertyName("data")]
        public FortiWebContentRoutingCreateData Data { get; set; }
    }
}
