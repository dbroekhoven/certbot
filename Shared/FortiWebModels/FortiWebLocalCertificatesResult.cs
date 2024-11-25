using System;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebLocalCertificatesResult
    {
        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("q_ref")]
        public int QRef { get; set; }

        [JsonPropertyName("cert_type")]
        public string CertType { get; set; }

        [JsonPropertyName("pkey_type")]
        public int PkeyType { get; set; }

        [JsonPropertyName("can_delete")]
        public bool CanDelete { get; set; }

        [JsonPropertyName("can_view")]
        public bool CanView { get; set; }

        [JsonPropertyName("can_download")]
        public bool CanDownload { get; set; }

        [JsonPropertyName("can_config")]
        public bool CanConfig { get; set; }

        [JsonPropertyName("is_default")]
        public bool IsDefault { get; set; }

        [JsonPropertyName("hsm")]
        public string Hsm { get; set; }

        [JsonPropertyName("comments")]
        public string Comments { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }

        [JsonPropertyName("validFrom")]
        public string ValidFrom { get; set; }

        [JsonPropertyName("validTo")]
        public string ValidToString { get; set; }

        [JsonIgnore]
        public DateTime ValidTo => DateTime.ParseExact(this.ValidToString[..10], "yyyy-MM-dd", CultureInfo.InvariantCulture);

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("serialNumber")]
        public string SerialNumber { get; set; }

        [JsonPropertyName("extension")]
        public string Extension { get; set; }
    }
}
