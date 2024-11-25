using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerNameIndications
    {
        [JsonPropertyName("results")]
        public List<FortiWebServerNameIndicationsResult> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
