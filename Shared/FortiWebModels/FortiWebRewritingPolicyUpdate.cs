using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebRewritingPolicyUpdate
    {
        [JsonPropertyName("data")]
        public FortiWebRewritingPolicyUpdateData Data { get; set; }
    }
}
