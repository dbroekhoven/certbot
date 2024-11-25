using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebStatus
    {
        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("results")]
        public FortiWebStatusResult Result { get; set; }

        [JsonIgnore]
        public string Json { get; set; }

        [JsonIgnore]
        public bool HasError => this.ErrorCode > 0;
    }
}
