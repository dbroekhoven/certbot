using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPolicyFtpUpdateData
    {
        [JsonPropertyName("protocol")]
        public string Protocol { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("deployment-mode")]
        public string DeploymentMode { get; set; }

        [JsonPropertyName("ssl")]
        public string Ssl { get; set; }

        [JsonPropertyName("implicit_ssl")]
        public string ImplicitSsl { get; set; }

        [JsonPropertyName("intermediate-certificate-group")]
        public string IntermediateCertificateGroup { get; set; }

        [JsonPropertyName("monitor-mode")]
        public string MonitorMode { get; set; }

        [JsonPropertyName("syncookie")]
        public string Syncookie { get; set; }

        [JsonPropertyName("half-open-threshold")]
        public int HalfOpenThreshold { get; set; }

        [JsonPropertyName("comment")]
        public string Comment { get; set; }

        [JsonPropertyName("tls-v10")]
        public string TlsV10 { get; set; }

        [JsonPropertyName("tls-v11")]
        public string TlsV11 { get; set; }

        [JsonPropertyName("tls-v12")]
        public string TlsV12 { get; set; }

        [JsonPropertyName("tls-v13")]
        public string TlsV13 { get; set; }

        [JsonPropertyName("ssl-cipher")]
        public string SslCipher { get; set; }

        [JsonPropertyName("ssl-custom-cipher")]
        public string SslCustomCipher { get; set; }

        [JsonPropertyName("tls13-custom-cipher")]
        public string Tls13CustomCipher { get; set; }

        [JsonPropertyName("ssl-noreg")]
        public string SslNoreg { get; set; }

        [JsonPropertyName("vserver")]
        public string Vserver { get; set; }

        [JsonPropertyName("server-pool")]
        public string ServerPool { get; set; }

        [JsonPropertyName("service")]
        public string Service { get; set; }

        [JsonPropertyName("certificate")]
        public string Certificate { get; set; }

        [JsonPropertyName("ftp-protection-profile")]
        public string FtpProtectionProfile { get; set; }
    }
}
