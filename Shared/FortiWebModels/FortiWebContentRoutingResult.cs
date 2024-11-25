using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebContentRoutingResult
    {
        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("can_view")]
        public int CanView { get; set; }

        [JsonPropertyName("q_ref")]
        public int QRef { get; set; }

        [JsonPropertyName("can_clone")]
        public int CanClone { get; set; }

        [JsonPropertyName("q_type")]
        public int QType { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("server-pool")]
        public string ServerPool { get; set; }

        [JsonPropertyName("server-pool_val")]
        public string ServerPoolVal { get; set; }

        [JsonPropertyName("http-content-routing-id")]
        public string HttpContentRoutingId { get; set; }

        [JsonPropertyName("sz_content-routing-match-list")]
        public int SzContentRoutingMatchList { get; set; }
    }
}
