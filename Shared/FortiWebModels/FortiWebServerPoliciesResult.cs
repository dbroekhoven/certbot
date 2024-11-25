using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPoliciesResult
    {
        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

    }
}
