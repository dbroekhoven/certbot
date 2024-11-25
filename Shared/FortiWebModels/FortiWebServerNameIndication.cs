using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerNameIndication
    {
        [JsonPropertyName("results")]
        public FortiWebServerNameIndicationsResult Result { get; set; }

        [JsonIgnore]
        public string Json { get; set; }

        [JsonIgnore]
        public bool HasError => this.Result.ErrorCode > 0;
    }
}
