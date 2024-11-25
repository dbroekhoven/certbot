using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebIpMemberDeleteData
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
