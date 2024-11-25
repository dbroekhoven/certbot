using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerNameIndicationCreate
    {
        [JsonPropertyName("data")]
        public FortiWebServerNameIndicationCreateData Data { get; set; }
    }
}
