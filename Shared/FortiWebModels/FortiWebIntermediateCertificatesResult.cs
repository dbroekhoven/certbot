using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebIntermediateCertificatesResult
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("issuer")]
        public string Issuer { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("serialNumber")]
        public string RawSerialNumber { get; set; }

        public string GetSerialNumber()
        {
            return this.RawSerialNumber.Replace(":", "").Replace(" ", "").ToLowerInvariant();
        }
    }
}
