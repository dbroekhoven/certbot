using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebRewritingRuleCreateData
    {
        [JsonPropertyName("q_type")]
        public int QType { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("action_val")]
        public string ActionVal { get; set; }

        [JsonPropertyName("host-status_val")]
        public string HostStatusVal { get; set; }

        [JsonPropertyName("host-use-pserver_val")]
        public string HostUsePserverVal { get; set; }

        [JsonPropertyName("url-status_val")]
        public string UrlStatusVal { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("referer-status_val")]
        public string RefererStatusVal { get; set; }

        [JsonPropertyName("referer-use-pserver_val")]
        public string RefererUsePserverVal { get; set; }

        [JsonPropertyName("location_replace")]
        public string LocationReplace { get; set; }

        [JsonPropertyName("location-status")]
        public string LocationStatus { get; set; }

        [JsonPropertyName("location-status_val")]
        public string LocationStatusVal { get; set; }

        [JsonPropertyName("header-status_val")]
        public string HeaderStatusVal { get; set; }

        [JsonPropertyName("response-header-status")]
        public string ResponseHeaderStatus { get; set; }

        [JsonPropertyName("response-header-status_val")]
        public string ResponseHeaderStatusVal { get; set; }

        [JsonPropertyName("response-header-name")]
        public string ResponseHeaderName { get; set; }

        [JsonPropertyName("response-header-value")]
        public string ResponseHeaderValue { get; set; }

        [JsonPropertyName("sz_header-removal")]
        public int SzHeaderRemoval { get; set; }

        [JsonPropertyName("sz_response-header-removal")]
        public int SzResponseHeaderRemoval { get; set; }

        [JsonPropertyName("flag_operation")]
        public int FlagOperation { get; set; }

        [JsonPropertyName("sz_match-condition")]
        public int SzMatchCondition { get; set; }

        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        [JsonIgnore]
        public string Json { get; set; }

        [JsonIgnore]
        public bool HasError => this.ErrorCode > 0;
    }
}
