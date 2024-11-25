using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebIpMemberResult
    {
        [JsonPropertyName("results")]
        public List<FortiWebIpMember> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
