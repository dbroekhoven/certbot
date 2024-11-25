using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebIpMemberCreateResult
    {
        [JsonPropertyName("results")]
        public FortiWebIpMember Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
