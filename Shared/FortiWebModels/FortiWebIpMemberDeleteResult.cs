using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebIpMemberDeleteResult
    {
        [JsonPropertyName("results")]
        public FortiWebIpMemberDeleteData Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
