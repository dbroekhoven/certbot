using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebRewritingRulesResult
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

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("action_val")]
        public string ActionVal { get; set; }

        [JsonPropertyName("host-status")]
        public string HostStatus { get; set; }

        [JsonPropertyName("host-status_val")]
        public string HostStatusVal { get; set; }

        [JsonPropertyName("host-use-pserver")]
        public string HostUsePserver { get; set; }

        [JsonPropertyName("host-use-pserver_val")]
        public string HostUsePserverVal { get; set; }

        [JsonPropertyName("host")]
        public string Host { get; set; }

        [JsonPropertyName("url-status")]
        public string UrlStatus { get; set; }

        [JsonPropertyName("url-status_val")]
        public string UrlStatusVal { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

        [JsonPropertyName("referer-status")]
        public string RefererStatus { get; set; }

        [JsonPropertyName("referer-status_val")]
        public string RefererStatusVal { get; set; }

        [JsonPropertyName("referer-use-pserver")]
        public string RefererUsePserver { get; set; }

        [JsonPropertyName("referer-use-pserver_val")]
        public string RefererUsePserverVal { get; set; }

        [JsonPropertyName("referer")]
        public string Referer { get; set; }

        [JsonPropertyName("body_replace")]
        public string BodyReplace { get; set; }

        [JsonPropertyName("location_replace")]
        public string LocationReplace { get; set; }

        [JsonPropertyName("location-status")]
        public string LocationStatus { get; set; }

        [JsonPropertyName("location-status_val")]
        public string LocationStatusVal { get; set; }

        [JsonPropertyName("header-status")]
        public string HeaderStatus { get; set; }

        [JsonPropertyName("header-status_val")]
        public string HeaderStatusVal { get; set; }

        [JsonPropertyName("header-name")]
        public string HeaderName { get; set; }

        [JsonPropertyName("header-value")]
        public string HeaderValue { get; set; }

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
    }
}
