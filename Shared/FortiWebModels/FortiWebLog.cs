using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebLog
    {
        [JsonPropertyName("results")]
        public FortiWebLogResults Results { get; set; }
    }

    public class FortiWebLogResults
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("filter_offset")]
        public int FilterOffset { get; set; }

        [JsonPropertyName("payload")]
        public List<FortiWebLogItem> Payload { get; set; }
    }

    public class FortiWebLogItem
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("time")]
        public string Time { get; set; }

        //[JsonPropertyName("timezone")]
        //public string Timezone { get; set; }

        //[JsonPropertyName("device_id")]
        //public string DeviceId { get; set; }

        //[JsonPropertyName("log_id")]
        //public string LogId { get; set; }

        //[JsonPropertyName("msg_id")]
        //public string MsgId { get; set; }

        //[JsonPropertyName("http_session_id")]
        //public string HttpSessionId { get; set; }

        //[JsonPropertyName("dev_id")]
        //public string DevId { get; set; }

        [JsonPropertyName("policy")]
        public string Policy { get; set; }

        //[JsonPropertyName("content_switch_name")]
        //public string ContentSwitchName { get; set; }

        [JsonPropertyName("server_pool_name")]
        public string ServerPoolName { get; set; }

        //[JsonPropertyName("proto")]
        //public string Proto { get; set; }

        [JsonPropertyName("service")]
        public string Service { get; set; }

        //[JsonPropertyName("backend_service")]
        //public string BackendService { get; set; }

        //[JsonPropertyName("cipher_suite")]
        //public string CipherSuite { get; set; }

        //[JsonPropertyName("http_version")]
        //public string HttpVersion { get; set; }

        [JsonPropertyName("http_host")]
        public string HttpHost { get; set; }

        //[JsonPropertyName("http_method")]
        //public string HttpMethod { get; set; }

        [JsonPropertyName("http_url")]
        public string HttpUrl { get; set; }

        //[JsonPropertyName("http_refer")]
        //public string HttpRefer { get; set; }

        [JsonPropertyName("http_agent")]
        public string HttpAgent { get; set; }

        //[JsonPropertyName("user_name")]
        //public string UserName { get; set; }

        //[JsonPropertyName("monitor_status")]
        //public string MonitorStatus { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("severity_level")]
        public string SeverityLevel { get; set; }

        //[JsonPropertyName("threat_level")]
        //public string ThreatLevel { get; set; }

        //[JsonPropertyName("client_level")]
        //public string ClientLevel { get; set; }

        //[JsonPropertyName("threat_weight")]
        //public string ThreatWeight { get; set; }

        //[JsonPropertyName("history_threat_weight")]
        //public string HistoryThreatWeight { get; set; }

        [JsonPropertyName("srccountry")]
        public string Srccountry { get; set; }

        //[JsonPropertyName("country_flag")]
        //public string CountryFlag { get; set; }

        //[JsonPropertyName("signature_cve_id")]
        //public string SignatureCveId { get; set; }

        //[JsonPropertyName("owasp_top10")]
        //public string OwaspTop10 { get; set; }

        [JsonPropertyName("main_type")]
        public string MainType { get; set; }

        [JsonPropertyName("sub_type")]
        public string SubType { get; set; }

        //[JsonPropertyName("signature_subclass")]
        //public string SignatureSubclass { get; set; }

        //[JsonPropertyName("signature_id")]
        //public string SignatureId { get; set; }

        //[JsonPropertyName("msg_src")]
        //public string MsgSrc { get; set; }

        //[JsonPropertyName("msg")]
        //public string Msg { get; set; }

        //[JsonPropertyName("ml_domain_index")]
        //public string MlDomainIndex { get; set; }

        //[JsonPropertyName("ml_url_dbid")]
        //public string MlUrlDbid { get; set; }

        //[JsonPropertyName("ml_arg_dbid")]
        //public string MlArgDbid { get; set; }

        //[JsonPropertyName("detail")]
        //public string Detail { get; set; }

        //[JsonPropertyName("type")]
        //public string Type { get; set; }

        //[JsonPropertyName("subtype")]
        //public string Subtype { get; set; }

        //[JsonPropertyName("pri")]
        //public string Pri { get; set; }

        [JsonPropertyName("src")]
        public string Src { get; set; }

        //[JsonPropertyName("src_port")]
        //public string SrcPort { get; set; }

        [JsonPropertyName("dst")]
        public string Dst { get; set; }

        //[JsonPropertyName("dst_port")]
        //public string DstPort { get; set; }

        //[JsonPropertyName("trigger_policy")]
        //public string TriggerPolicy { get; set; }

        //[JsonPropertyName("false_positive_mitigation")]
        //public string FalsePositiveMitigation { get; set; }

        //[JsonPropertyName("event_score")]
        //public string EventScore { get; set; }

        //[JsonPropertyName("score_message")]
        //public string ScoreMessage { get; set; }

        //[JsonPropertyName("entry_sequence")]
        //public string EntrySequence { get; set; }

        //[JsonPropertyName("ml_log_hmm_probability")]
        //public string MlLogHmmProbability { get; set; }

        //[JsonPropertyName("ml_log_sample_prob_mean")]
        //public string MlLogSampleProbMean { get; set; }

        //[JsonPropertyName("ml_log_sample_arglen_mean")]
        //public string MlLogSampleArglenMean { get; set; }

        //[JsonPropertyName("ml_log_arglen")]
        //public string MlLogArglen { get; set; }

        //[JsonPropertyName("ml_svm_log_main_types")]
        //public string MlSvmLogMainTypes { get; set; }

        //[JsonPropertyName("ml_svm_log_match_types")]
        //public string MlSvmLogMatchTypes { get; set; }

        //[JsonPropertyName("ml_svm_accuracy")]
        //public string MlSvmAccuracy { get; set; }

        //[JsonPropertyName("ml_allow_method")]
        //public string MlAllowMethod { get; set; }

        //[JsonPropertyName("rel_time")]
        //public string RelTime { get; set; }

        //[JsonPropertyName("except")]
        //public string Except { get; set; }

        [JsonPropertyName("http_policy")]
        public string HttpPolicy { get; set; }

        //[JsonPropertyName("except_url")]
        //public string ExceptUrl { get; set; }

        //[JsonPropertyName("signature_id_exist")]
        //public int SignatureIdExist { get; set; }

        //[JsonPropertyName("global_status")]
        //public string GlobalStatus { get; set; }

        //[JsonPropertyName("fpm")]
        //public string Fpm { get; set; }

        //[JsonPropertyName("fpm_url")]
        //public string FpmUrl { get; set; }

        //[JsonPropertyName("alert_only")]
        //public string AlertOnly { get; set; }

        //[JsonPropertyName("alert_only_url")]
        //public string AlertOnlyUrl { get; set; }

        [JsonPropertyName("signature")]
        public string SignaturePolicy { get; set; }

        //[JsonPropertyName("pcap_id")]
        //public int PcapId { get; set; }

        //[JsonPropertyName("logfile")]
        //public string Logfile { get; set; }

        //[JsonPropertyName("id")]
        //public int Id { get; set; }

        //[JsonPropertyName("length")]
        //public int Length { get; set; }

        //[JsonPropertyName("fpos")]
        //public int Fpos { get; set; }

        //[JsonPropertyName("flag_val")]
        //public int FlagVal { get; set; }

        //[JsonPropertyName("flag")]
        //public string Flag { get; set; }

        //[JsonPropertyName("has_comment")]
        //public string HasComment { get; set; }
    }
}
