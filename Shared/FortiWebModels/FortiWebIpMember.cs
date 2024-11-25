using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebIpMember
    {
        [JsonPropertyName("seq")]
        public int Seq { get; set; }

        [JsonPropertyName("q_type")]
        public int QType { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("type_val")]
        public string TypeVal { get; set; }

        [JsonPropertyName("group-type")]
        public string GroupType { get; set; }

        [JsonPropertyName("group-type_val")]
        public string GroupTypeVal { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("ip-group")]
        public string IpGroup { get; set; }

        [JsonPropertyName("ip-group_val")]
        public string IpGroupVal { get; set; }
    }
}