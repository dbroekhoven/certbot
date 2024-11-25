using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebIpMemberCreate
    {
        [JsonPropertyName("data")]
        public FortiWebIpMember Data { get; set; }
    }
}
