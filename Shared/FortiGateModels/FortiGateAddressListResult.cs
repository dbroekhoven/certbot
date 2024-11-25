using System.Text.Json.Serialization;

namespace Shared.FortiGateModels
{
    public class FortiGateAddressListResult
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("q_origin_key")]
        public string OriginKey { get; set; }

        [JsonPropertyName("css-class")]
        public string CssClass { get; set; }

        [JsonPropertyName("uuid")]
        public string Uuid { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("q_ref")]
        public int Ref { get; set; }

        [JsonPropertyName("q_static")]
        public bool Static { get; set; }

        [JsonPropertyName("q_no_rename")]
        public bool NoRename { get; set; }

        [JsonPropertyName("q_global_entry")]
        public bool GlobalEntry { get; set; }

        [JsonPropertyName("q_path")]
        public string Path { get; set; }

        [JsonPropertyName("q_name")]
        public string QName { get; set; }

        [JsonPropertyName("q_mkey_type")]
        public string MkeyType { get; set; }

        [JsonPropertyName("q_no_edit")]
        public bool NoEdit { get; set; }

        [JsonPropertyName("q_class")]
        public string QClass { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("sub-type")]
        public string SubType { get; set; }

        [JsonPropertyName("start-ip")]
        public string StartIp { get; set; }

        [JsonPropertyName("end-ip")]
        public string EndIp { get; set; }

        [JsonPropertyName("color")]
        public int? Color { get; set; }

        [JsonPropertyName("subnet")]
        public string Subnet { get; set; }

        [JsonPropertyName("associated-interface")]
        public FortiGateAddressInterface Interface { get; set; }
    }
}
