using System.Text.Json.Serialization;

namespace Shared.FortiWebModels
{
    public class FortiWebServerPoolRulesResult
    {
        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("seq")]
        public int Seq { get; set; }

        [JsonPropertyName("q_type")]
        public int QType { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("server-type")]
        public string ServerType { get; set; }

        [JsonPropertyName("server-type_val")]
        public string ServerTypeVal { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        [JsonPropertyName("domain")]
        public string Domain { get; set; }

        [JsonPropertyName("adfs-username")]
        public string AdfsUsername { get; set; }

        [JsonPropertyName("adfs-password")]
        public string AdfsPassword { get; set; }

        [JsonPropertyName("port")]
        public int Port { get; set; }

        [JsonPropertyName("weight")]
        public int Weight { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("status_val")]
        public string StatusVal { get; set; }

        [JsonPropertyName("server-id")]
        public long ServerId { get; set; }

        [JsonPropertyName("backup-server")]
        public string BackupServer { get; set; }

        [JsonPropertyName("backup-server_val")]
        public string BackupServerVal { get; set; }

        [JsonPropertyName("proxy-protocol")]
        public string ProxyProtocol { get; set; }

        [JsonPropertyName("proxy-protocol_val")]
        public string ProxyProtocolVal { get; set; }

        [JsonPropertyName("proxy-protocol-version")]
        public string ProxyProtocolVersion { get; set; }

        [JsonPropertyName("proxy-protocol-version_val")]
        public string ProxyProtocolVersionVal { get; set; }

        [JsonPropertyName("ssl")]
        public string Ssl { get; set; }

        [JsonPropertyName("ssl_val")]
        public string SslVal { get; set; }

        [JsonPropertyName("implicit_ssl")]
        public string ImplicitSsl { get; set; }

        [JsonPropertyName("implicit_ssl_val")]
        public string ImplicitSslVal { get; set; }

        [JsonPropertyName("ssl-quiet-shutdown")]
        public string SslQuietShutdown { get; set; }

        [JsonPropertyName("ssl-quiet-shutdown_val")]
        public string SslQuietShutdownVal { get; set; }

        [JsonPropertyName("ssl-session-timeout")]
        public int SslSessionTimeout { get; set; }

        [JsonPropertyName("server-side-sni")]
        public string ServerSideSni { get; set; }

        [JsonPropertyName("server-side-sni_val")]
        public string ServerSideSniVal { get; set; }

        [JsonPropertyName("multi-certificate")]
        public string MultiCertificate { get; set; }

        [JsonPropertyName("multi-certificate_val")]
        public string MultiCertificateVal { get; set; }

        [JsonPropertyName("certificate")]
        public string Certificate { get; set; }

        [JsonPropertyName("certificate_val")]
        public string CertificateVal { get; set; }

        [JsonPropertyName("certificate-group")]
        public string CertificateGroup { get; set; }

        [JsonPropertyName("certificate-group_val")]
        public string CertificateGroupVal { get; set; }

        [JsonPropertyName("intermediate-certificate-group")]
        public string IntermediateCertificateGroup { get; set; }

        [JsonPropertyName("intermediate-certificate-group_val")]
        public string IntermediateCertificateGroupVal { get; set; }

        [JsonPropertyName("certificate-verify")]
        public string CertificateVerify { get; set; }

        [JsonPropertyName("certificate-verify_val")]
        public string CertificateVerifyVal { get; set; }

        [JsonPropertyName("client-certificate-proxy")]
        public string ClientCertificateProxy { get; set; }

        [JsonPropertyName("client-certificate-proxy_val")]
        public string ClientCertificateProxyVal { get; set; }

        [JsonPropertyName("client-certificate-proxy-sign-ca")]
        public string ClientCertificateProxySignCa { get; set; }

        [JsonPropertyName("client-certificate-proxy-sign-ca_val")]
        public string ClientCertificateProxySignCaVal { get; set; }

        [JsonPropertyName("client-certificate")]
        public string ClientCertificate { get; set; }

        [JsonPropertyName("client-certificate_val")]
        public string ClientCertificateVal { get; set; }

        [JsonPropertyName("server-certificate-verify")]
        public string ServerCertificateVerify { get; set; }

        [JsonPropertyName("server-certificate-verify_val")]
        public string ServerCertificateVerifyVal { get; set; }

        [JsonPropertyName("server-certificate-verify-policy")]
        public string ServerCertificateVerifyPolicy { get; set; }

        [JsonPropertyName("server-certificate-verify-policy_val")]
        public string ServerCertificateVerifyPolicyVal { get; set; }

        [JsonPropertyName("server-certificate-verify-action")]
        public string ServerCertificateVerifyAction { get; set; }

        [JsonPropertyName("server-certificate-verify-action_val")]
        public string ServerCertificateVerifyActionVal { get; set; }

        [JsonPropertyName("session-ticket-reuse")]
        public string SessionTicketReuse { get; set; }

        [JsonPropertyName("session-ticket-reuse_val")]
        public string SessionTicketReuseVal { get; set; }

        [JsonPropertyName("session-id-reuse")]
        public string SessionIdReuse { get; set; }

        [JsonPropertyName("session-id-reuse_val")]
        public string SessionIdReuseVal { get; set; }

        [JsonPropertyName("sni")]
        public string Sni { get; set; }

        [JsonPropertyName("sni_val")]
        public string SniVal { get; set; }

        [JsonPropertyName("sni-certificate")]
        public string SniCertificate { get; set; }

        [JsonPropertyName("sni-certificate_val")]
        public string SniCertificateVal { get; set; }

        [JsonPropertyName("sni-strict")]
        public string SniStrict { get; set; }

        [JsonPropertyName("sni-strict_val")]
        public string SniStrictVal { get; set; }

        [JsonPropertyName("urlcert")]
        public string Urlcert { get; set; }

        [JsonPropertyName("urlcert_val")]
        public string UrlcertVal { get; set; }

        [JsonPropertyName("urlcert-group")]
        public string UrlcertGroup { get; set; }

        [JsonPropertyName("urlcert-group_val")]
        public string UrlcertGroupVal { get; set; }

        [JsonPropertyName("urlcert-hlen")]
        public int UrlcertHlen { get; set; }

        [JsonPropertyName("tls-v10")]
        public string TlsV10 { get; set; }

        [JsonPropertyName("tls-v10_val")]
        public string TlsV10Val { get; set; }

        [JsonPropertyName("tls-v11")]
        public string TlsV11 { get; set; }

        [JsonPropertyName("tls-v11_val")]
        public string TlsV11Val { get; set; }

        [JsonPropertyName("tls-v12")]
        public string TlsV12 { get; set; }

        [JsonPropertyName("tls-v12_val")]
        public string TlsV12Val { get; set; }

        [JsonPropertyName("tls-v13")]
        public string TlsV13 { get; set; }

        [JsonPropertyName("tls-v13_val")]
        public string TlsV13Val { get; set; }

        [JsonPropertyName("ssl-noreg")]
        public string SslNoreg { get; set; }

        [JsonPropertyName("ssl-noreg_val")]
        public string SslNoregVal { get; set; }

        [JsonPropertyName("ssl-cipher")]
        public string SslCipher { get; set; }

        [JsonPropertyName("ssl-cipher_val")]
        public string SslCipherVal { get; set; }

        [JsonPropertyName("ssl-custom-cipher")]
        public string SslCustomCipher { get; set; }

        [JsonPropertyName("ssl-custom-cipher_val")]
        public string SslCustomCipherVal { get; set; }

        [JsonPropertyName("tls13-custom-cipher")]
        public string Tls13CustomCipher { get; set; }

        [JsonPropertyName("tls13-custom-cipher_val")]
        public string Tls13CustomCipherVal { get; set; }

        [JsonPropertyName("hsts-header")]
        public string HstsHeader { get; set; }

        [JsonPropertyName("hsts-header_val")]
        public string HstsHeaderVal { get; set; }

        [JsonPropertyName("hsts-max-age")]
        public int HstsMaxAge { get; set; }

        [JsonPropertyName("hpkp-header")]
        public string HpkpHeader { get; set; }

        [JsonPropertyName("hpkp-header_val")]
        public string HpkpHeaderVal { get; set; }

        [JsonPropertyName("client-certificate-forwarding")]
        public string ClientCertificateForwarding { get; set; }

        [JsonPropertyName("client-certificate-forwarding_val")]
        public string ClientCertificateForwardingVal { get; set; }

        [JsonPropertyName("client-certificate-forwarding-sub-header")]
        public string ClientCertificateForwardingSubHeader { get; set; }

        [JsonPropertyName("client-certificate-forwarding-cert-header")]
        public string ClientCertificateForwardingCertHeader { get; set; }

        [JsonPropertyName("health-check-inherit")]
        public string HealthCheckInherit { get; set; }

        [JsonPropertyName("health-check-inherit_val")]
        public string HealthCheckInheritVal { get; set; }

        [JsonPropertyName("health")]
        public string Health { get; set; }

        [JsonPropertyName("health_val")]
        public string HealthVal { get; set; }

        [JsonPropertyName("conn-limit")]
        public int ConnLimit { get; set; }

        [JsonPropertyName("recover")]
        public int Recover { get; set; }

        [JsonPropertyName("warm-up")]
        public int WarmUp { get; set; }

        [JsonPropertyName("warm-rate")]
        public int WarmRate { get; set; }

        [JsonPropertyName("http2")]
        public string Http2 { get; set; }

        [JsonPropertyName("http2_val")]
        public string Http2Val { get; set; }

        [JsonPropertyName("http2-ssl-custom-cipher")]
        public string Http2SslCustomCipher { get; set; }

        [JsonPropertyName("http2-ssl-custom-cipher_val")]
        public string Http2SslCustomCipherVal { get; set; }

        [JsonPropertyName("hlck-domain")]
        public string HlckDomain { get; set; }
    }
}
