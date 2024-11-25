using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebIntermediateGroups
    {
        [JsonPropertyName("results")]
        public List<FortiWebIntermediateGroupsResult> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
