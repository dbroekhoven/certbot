using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebContentRoutingRulesResult
    {
        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("seq")]
        public int Seq { get; set; }

        [JsonPropertyName("q_type")]
        public int QType { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("match-object")]
        public string MatchObject { get; set; }

        [JsonPropertyName("match-object_val")]
        public string MatchObjectVal { get; set; }

        [JsonPropertyName("match-condition")]
        public string MatchCondition { get; set; }

        [JsonPropertyName("match-condition_val")]
        public string MatchConditionVal { get; set; }

        [JsonPropertyName("x509-subject-name")]
        public string X509SubjectName { get; set; }

        [JsonPropertyName("x509-subject-name_val")]
        public string X509SubjectNameVal { get; set; }

        [JsonPropertyName("match-expression")]
        public string MatchExpression { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("concatenate")]
        public string Concatenate { get; set; }

        [JsonPropertyName("concatenate_val")]
        public string ConcatenateVal { get; set; }

        [JsonPropertyName("name-match-condition")]
        public string NameMatchCondition { get; set; }

        [JsonPropertyName("name-match-condition_val")]
        public string NameMatchConditionVal { get; set; }

        [JsonPropertyName("value-match-condition")]
        public string ValueMatchCondition { get; set; }

        [JsonPropertyName("value-match-condition_val")]
        public string ValueMatchConditionVal { get; set; }

        [JsonPropertyName("start-ip")]
        public string StartIp { get; set; }

        [JsonPropertyName("end-ip")]
        public string EndIp { get; set; }

        [JsonPropertyName("reverse")]
        public string Reverse { get; set; }

        [JsonPropertyName("reverse_val")]
        public string ReverseVal { get; set; }

        [JsonPropertyName("country-list")]
        public string CountryList { get; set; }

        [JsonPropertyName("ip-list")]
        public string IpList { get; set; }

        [JsonPropertyName("ip-list-file")]
        public string IpListFile { get; set; }
    }
}
