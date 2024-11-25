using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebLocalCertificates
    {
        [JsonPropertyName("results")]
        public List<FortiWebLocalCertificatesResult> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
