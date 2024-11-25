using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebIntermediateCertificates
    {
        [JsonPropertyName("results")]
        public List<FortiWebIntermediateCertificatesResult> Results { get; set; }

        [JsonIgnore]
        public string Json { get; set; }
    }
}
