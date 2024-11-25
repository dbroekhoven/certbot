using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Cre8ion.Common;
using Shared.FortiWebModels;

namespace Shared.Services
{
    public class FortiWebService : IService
    {
        private readonly HttpService httpService;

        public FortiWebService(HttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<FortiWebStatus> GetStatusAsync()
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/system/status.systemstatus";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebStatus>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerPools> GetServerPoolsAsync()
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/server-pool";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebServerPools>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerPool> CreateServerPoolAsync(string serverPoolName, bool loadBalanced)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/server-pool";

            var data = new FortiWebServerPoolCreate
            {
                Data = new FortiWebServerPoolCreateData
                {
                    Name = serverPoolName,
                    Health = "",
                    Type = "reverse-proxy",
                    ServerBalance = loadBalanced ? "enable" : "disable",
                    LoadBalanceMethod = "round-robin",
                    Comment = string.Empty,
                    HttpReuse = "always",
                    ReuseConnMaxRequest = 1000,
                    ReuseConnMaxCount = 1000
                }
            };

            var response = await this.httpService.PostAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebServerPool>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerPool> DeleteServerPoolAsync(string serverPoolName)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/server-pool?mkey={serverPoolName}";

            var response = await this.httpService.DeleteAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));

            var result = JsonSerializer.Deserialize<FortiWebServerPool>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerPoolRules> GetServerPoolRulesAsync(string serverPoolName)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/server-pool/pserver-list?mkey={serverPoolName}";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebServerPoolRules>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerPoolRule> CreateServerPoolRuleAsync(string serverPoolName, string internalAddress)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/server-pool/pserver-list?mkey={serverPoolName}";

            var data = new FortiWebServerPoolRuleCreate
            {
                Data = new FortiWebServerPoolRuleCreateData
                {
                    Status = "enable",
                    Type = "physical",
                    Address = internalAddress,
                    Http2 = "disable",
                    HealthCheckInherit = "enable"
                }
            };

            var response = await this.httpService.PostAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebServerPoolRule>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerPoolRule> UpdateServerPoolRuleAsync(string serverPoolName, string internalAddress, string ruleId)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/server-pool/pserver-list?mkey={serverPoolName}&sub_mkey={ruleId}";

            var data = new FortiWebServerPoolRuleCreate
            {
                Data = new FortiWebServerPoolRuleCreateData
                {
                    Status = "enable",
                    Type = "physical",
                    Address = internalAddress,
                    Http2 = "disable"
                }
            };

            var response = await this.httpService.PutAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebServerPoolRule>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerPoolRule> DeleteServerPoolRuleAsync(string serverPoolName, string ruleId)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/server-pool/pserver-list?mkey={serverPoolName}&sub_mkey={ruleId}";

            var response = await this.httpService.DeleteAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebServerPoolRule>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebContentRoutings> GetContentRoutingsAsync()
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/http-content-routing-policy";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebContentRoutings>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebContentRouting> CreateContentRoutingAsync(string serverPoolName, string contentRoutingName)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/http-content-routing-policy";

            var data = new FortiWebContentRoutingCreate
            {
                Data = new FortiWebContentRoutingCreateData
                {
                    Name = contentRoutingName,
                    ServerPool = serverPoolName
                }
            };

            var response = await this.httpService.PostAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebContentRouting>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebContentRouting> UpdateContentRoutingAsync(string serverPoolName, string contentRoutingName)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/http-content-routing-policy?mkey={contentRoutingName}";

            var data = new FortiWebContentRoutingCreate
            {
                Data = new FortiWebContentRoutingCreateData
                {
                    Name = contentRoutingName,
                    ServerPool = serverPoolName
                }
            };

            var response = await this.httpService.PutAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebContentRouting>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebContentRouting> DeleteContentRoutingAsync(string contentRoutingName)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/http-content-routing-policy?mkey={contentRoutingName}";

            var response = await this.httpService.DeleteAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebContentRouting>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebContentRoutingRules> GetContentRoutingRulesAsync(string contentRoutingName)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/http-content-routing-policy/content-routing-match-list?mkey={contentRoutingName}";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebContentRoutingRules>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebContentRoutingRule> CreateContentRoutingRuleAsync(string contentRoutingName, string domainName)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/http-content-routing-policy/content-routing-match-list?mkey={contentRoutingName}";

            var data = new FortiWebContentRoutingRuleCreate
            {
                Data = new FortiWebContentRoutingRuleCreateData
                {
                    MatchObject = "http-host",
                    MatchCondition = "equal",
                    MatchExpression = domainName,
                    Concatenate = "or"
                }
            };

            var response = await this.httpService.PostAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebContentRoutingRule>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerPolicyRule> DeleteContentRoutingRuleAsync(string contentRoutingName, string ruleId)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/http-content-routing-policy/content-routing-match-list?mkey={contentRoutingName}&sub_mkey={ruleId}";

            var response = await this.httpService.DeleteAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebServerPolicyRule>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerPolicyRules> GetServerPolicyRulesAsync(string serverPolicyName)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/policy/http-content-routing-list?mkey={serverPolicyName}";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebServerPolicyRules>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerPolicies> GetServerPoliciesAsync()
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/policy";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebServerPolicies>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerPolicyFtpUpdate> UpdateFtpServerPolicyAsync(string serverPolicyName, string intermediateGroup, string certificateName)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/policy?mkey={serverPolicyName}";

            var data = new FortiWebServerPolicyFtpUpdate
            {
                Data = new FortiWebServerPolicyFtpUpdateData
                {
                    Name = serverPolicyName,
                    Certificate = certificateName,
                    DeploymentMode = "server-pool",
                    FtpProtectionProfile = "ftp-security-policy",
                    HalfOpenThreshold = 8192,
                    ImplicitSsl = "enable",
                    IntermediateCertificateGroup = intermediateGroup,
                    MonitorMode = "disable",
                    Protocol = "FTP",
                    ServerPool = "sp-ftps-cre8ion-nl-ftp",
                    Service = "FTPS",
                    Ssl = "enable",
                    SslCipher = "medium",
                    SslCustomCipher = "ECDHE-ECDSA-AES256-GCM-SHA384 ECDHE-RSA-AES256-GCM-SHA384 ECDHE-ECDSA-CHACHA20-POLY1305 ECDHE-RSA-CHACHA20-POLY1305 ECDHE-ECDSA-AES128-GCM-SHA256 ECDHE-RSA-AES128-GCM-SHA256 ECDHE-ECDSA-AES256-SHA384 ECDHE-RSA-AES256-SHA384 ECDHE-ECDSA-AES128-SHA256 ECDHE-RSA-AES128-SHA256 ECDHE-ECDSA-AES256-SHA ECDHE-RSA-AES256-SHA ECDHE-ECDSA-AES128-SHA ECDHE-RSA-AES128-SHA AES256-GCM-SHA384 AES128-GCM-SHA256 AES256-SHA256 AES128-SHA256 ",
                    SslNoreg = "enable",
                    Syncookie = "enable",
                    Tls13CustomCipher = "TLS_AES_256_GCM_SHA384 ",
                    TlsV10 = "disable",
                    TlsV11 = "disable",
                    TlsV12 = "enable",
                    TlsV13 = "enable",
                    Vserver = "vs-ftp-61"
                }
            };

            var response = await this.httpService.PutAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebServerPolicyFtpUpdate>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerPolicyRule> CreateServerPolicyRuleAsync(string serverPolicyName, string contentRoutingName, string protectionProfile)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/policy/http-content-routing-list?mkey={serverPolicyName}";

            var data = new FortiWebServerPolicyRuleCreate
            {
                Data = new FortiWebServerPolicyRuleCreateData
                {
                    ProfileInherit = "disable",
                    IsDefault = "no",
                    ContentRoutingPolicyName = contentRoutingName,
                    WebProtectionProfile = protectionProfile
                }
            };

            var response = await this.httpService.PostAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebServerPolicyRule>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerPolicyRule> UpdateServerPolicyRuleAsync(string serverPolicyName, string contentRoutingName, string protectionProfile, string contentRoutingId)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/policy/http-content-routing-list?mkey={serverPolicyName}&sub_mkey={contentRoutingId}";

            var data = new FortiWebServerPolicyRuleCreate
            {
                Data = new FortiWebServerPolicyRuleCreateData
                {
                    ProfileInherit = "disable",
                    IsDefault = "no",
                    ContentRoutingPolicyName = contentRoutingName,
                    WebProtectionProfile = protectionProfile
                }
            };

            var response = await this.httpService.PutAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebServerPolicyRule>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerPolicyRule> DeleteServerPolicyRuleAsync(string serverPolicyName, string contentRoutingId)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/policy/http-content-routing-list?mkey={serverPolicyName}&sub_mkey={contentRoutingId}";

            var response = await this.httpService.DeleteAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebServerPolicyRule>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebIntermediateGroups> GetIntermediateGroupsAsync()
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/system/certificate.intermediate-certificate-group";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebIntermediateGroups>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerNameIndications> GetServerNameIndicationsAsync(string serverNameIndication)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/system/certificate.sni/members?mkey={serverNameIndication}";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebServerNameIndications>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerNameIndication> CreateServerNameIndicationAsync(string serverNameIndication, string domainName, string certificateName, string intermediateGroup)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/system/certificate.sni/members?mkey={serverNameIndication}";

            var data = new FortiWebServerNameIndicationCreate
            {
                Data = new FortiWebServerNameIndicationCreateData
                {
                    DomainType = "plain",
                    Domain = domainName,
                    MultiLocalCert = "disable",
                    LocalCert = certificateName,
                    IntermediateGroup = intermediateGroup
                }
            };

            var response = await this.httpService.PostAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebServerNameIndication>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerNameIndication> UpdateServerNameIndicationAsync(string sniId, string serverNameIndication, string domainName, string certificateName, string intermediateGroup)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/system/certificate.sni/members?mkey={serverNameIndication}&sub_mkey={sniId}";

            var data = new FortiWebServerNameIndicationCreate
            {
                Data = new FortiWebServerNameIndicationCreateData
                {
                    DomainType = "plain",
                    Domain = domainName,
                    MultiLocalCert = "disable",
                    LocalCert = certificateName,
                    IntermediateGroup = intermediateGroup,
                }
            };

            var response = await this.httpService.PutAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebServerNameIndication>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebServerNameIndication> DeleteServerNameIndicationAsync(string serverNameIndication, string sniId)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/system/certificate.sni/members?mkey={serverNameIndication}&sub_mkey={sniId}";

            var response = await this.httpService.DeleteAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebServerNameIndication>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebLocalCertificates> GetLocalCertificatesAsync()
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/system/certificate.local";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebLocalCertificates>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebLocalCertificate> UploadLocalCertificateAsync(string certificateName, byte[] pfxBundle, string password)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/system/certificate.local.import_certificate";

            using var data = new MultipartFormDataContent
            {
                { new StringContent("PKCS12Certificate"), "type" },
                { new StringContent(password), "password" },
                { new ByteArrayContent(pfxBundle), "certificateWithKeyFile", $"{certificateName}.pfx" }
            };

            var response = await this.httpService.UploadAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebLocalCertificatesResult>(response);

            return new FortiWebLocalCertificate
            {
                Result = result,
                Json = response
            };
        }

        public async Task<FortiWebIntermediateCertificates> GetIntermediateCertificatesAsync()
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/system/certificate.intermediateca";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebIntermediateCertificates>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebLocalCertificate> UploadIntermediateCertificateAsync(byte[] caBundle)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/system/certificate.intermediateca";

            using var data = new MultipartFormDataContent
            {
                { new StringContent("localPC"), "type" },
                { new ByteArrayContent(caBundle), "uploadedFile", $"ca-bundle.crt" }
            };

            var response = await this.httpService.UploadAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebLocalCertificatesResult>(response);

            return new FortiWebLocalCertificate
            {
                Result = result,
                Json = response
            };
        }

        public async Task<FortiWebLocalCertificate> UploadLocalCertificateAsync(string certificateName, string certificate, string privateKey)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/system/certificate.local.import_certificate";

            using var data = new MultipartFormDataContent
            {
                { new StringContent("certificate"), "type" },
                { new StringContent(""), "password" },
                { new ByteArrayContent(Encoding.UTF8.GetBytes(certificate)), "certificateFile", $"{certificateName}.pem" },
                { new ByteArrayContent(Encoding.UTF8.GetBytes(privateKey)), "keyFile", $"{certificateName}.key" }
            };

            var response = await this.httpService.UploadAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebLocalCertificatesResult>(response);

            return new FortiWebLocalCertificate
            {
                Result = result,
                Json = response
            };
        }

        public async Task<FortiWebLocalCertificate> DeleteLocalCertificateAsync(string certificateName)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/system/certificate.local?mkey={certificateName}";

            var response = await this.httpService.DeleteAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebLocalCertificatesResult>(response);

            return new FortiWebLocalCertificate
            {
                Result = result,
                Json = response
            };
        }

        public async Task<FortiWebVirtualIPs> GetVirtualIPv6Async()
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/system/vip";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebVirtualIPs>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebVirtualIPsResult> CreateVirtualIPv6Async(string vipName, string ipAddress)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/system/vip";

            var data = new FortiWebVirtualIPsData
            {
                Data = new FortiWebVirtualIPsResult
                {
                    Interface = "port3",
                    Name = vipName,
                    Vip4 = "0.0.0.0/0",
                    Vip6 = $"{ipAddress}/128"
                }
            };

            var response = await this.httpService.PostAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebVirtualIPsResult>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebVirtualIPsResult> DeleteVirtualIPv6Async(string id)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/system/vip?mkey={id}";

            var response = await this.httpService.DeleteAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebVirtualIPsResult>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebVirtualServerAddress> GetVirtualServerVipsAsync(string name)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/vserver/vip-list?mkey={name}";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebVirtualServerAddress>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebVirtualServerAddressResult> UpdateVirtualServerAsync(string name, string vipName)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/vserver/vip-list?mkey={name}";

            var data = new FortiWebVirtualServerAddressData
            {
                Data = new FortiWebVirtualServerAddressResult
                {
                    Status = "enable",
                    UseInterfaceIP = "disable",
                    Vip = vipName
                }
            };

            var response = await this.httpService.PostAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebVirtualServerAddressResult>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebVirtualServerAddressResult> DeleteVirtualServerVipAsync(string name, string vipId)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/server-policy/vserver/vip-list?mkey={name}&sub_mkey={vipId}";

            var response = await this.httpService.DeleteAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebVirtualServerAddressResult>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebRewritePolicyRules> GetRewritePolicyRulesAsync()
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/waf/url-rewrite.url-rewrite-policy/rule?mkey=redirect-policy";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebRewritePolicyRules>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebRewritingRules> GetRewritingRulesAsync()
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/waf/url-rewrite.url-rewrite-rule";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebRewritingRules>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebRewritingRuleCreateData> CreateRewritingRuleAsync(string name, string redirectUrl)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/waf/url-rewrite.url-rewrite-rule";

            var data = new FortiWebRewritingRuleCreate
            {
                Data = new FortiWebRewritingRuleCreateData
                {
                    Action = "redirect",
                    ActionVal = "5",
                    FlagOperation = 0,
                    HeaderStatusVal = "0",
                    HostStatusVal = "0",
                    HostUsePserverVal = "0",
                    Location = redirectUrl,
                    LocationStatus = "disable",
                    LocationStatusVal = "0",
                    LocationReplace = "",
                    Name = name,
                    QType = 1,
                    RefererStatusVal = "0",
                    RefererUsePserverVal = "0",
                    ResponseHeaderName = "",
                    ResponseHeaderStatus = "disable",
                    ResponseHeaderStatusVal = "0",
                    ResponseHeaderValue = "",
                    SzHeaderRemoval = -1,
                    SzMatchCondition = -1,
                    SzResponseHeaderRemoval = -1,
                    UrlStatusVal = "0"
                }
            };

            var response = await this.httpService.PostAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebRewritingRuleCreateData>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebRewritingRuleCreateData> UpdateRewritingRuleAsync(string name, string redirectUrl)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/waf/url-rewrite.url-rewrite-rule?mkey={name}";

            var data = new FortiWebRewritingRuleCreate
            {
                Data = new FortiWebRewritingRuleCreateData
                {
                    Action = "redirect",
                    ActionVal = "5",
                    FlagOperation = 0,
                    HeaderStatusVal = "0",
                    HostStatusVal = "0",
                    HostUsePserverVal = "0",
                    Location = redirectUrl,
                    LocationStatus = "disable",
                    LocationStatusVal = "0",
                    LocationReplace = "",
                    Name = name,
                    QType = 1,
                    RefererStatusVal = "0",
                    RefererUsePserverVal = "0",
                    ResponseHeaderName = "",
                    ResponseHeaderStatus = "disable",
                    ResponseHeaderStatusVal = "0",
                    ResponseHeaderValue = "",
                    SzHeaderRemoval = -1,
                    SzMatchCondition = -1,
                    SzResponseHeaderRemoval = -1,
                    UrlStatusVal = "0"
                }
            };

            var response = await this.httpService.PutAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebRewritingRuleCreateData>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebRewritingRuleConditionCreateData> CreateRewritingRuleConditionAsync(string ruleName, string domainName)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/waf/url-rewrite.url-rewrite-rule/match-condition?mkey={ruleName}";

            var escapedValue = domainName
                .Replace(".", "\\.")
                .Replace("-", "\\-");

            var regexValue = $"^({escapedValue})$";

            var data = new FortiWebRewritingRuleConditionCreate
            {
                Data = new FortiWebRewritingRuleConditionCreateData
                {
                    HTTPProtocolVal = "1",
                    ContentTypeFilterVal = "0",
                    ContentTypeSetVal = "0",
                    Id = "0",
                    IsEssentialVal = "1",
                    ObjectText = "http-host",
                    ObjectValue = "1",
                    ProtocolFilter = "disable",
                    ProtocolFilterVal = "0",
                    QType = 0,
                    RegExp = regexValue,
                    ReverseMatch = "no",
                    ReverseMatchVal = "0",
                }
            };

            var response = await this.httpService.PostAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebRewritingRuleConditionCreateData>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebRewritingPolicyUpdateData> UpdateRewritingPolicyAsync(string ruleName)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/waf/url-rewrite.url-rewrite-policy/rule?mkey=redirect-policy";

            var data = new FortiWebRewritingPolicyUpdate
            {
                Data = new FortiWebRewritingPolicyUpdateData
                {
                    Id = "0",
                    QType = 0,
                    UrlRewriteRuleName = ruleName,
                    UrlRewriteRuleNameVal = "0"
                }
            };

            var response = await this.httpService.PostAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebRewritingPolicyUpdateData>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebRewritingPolicyUpdateData> DeleteFromRewritingPolicyAsync(string ruleId)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/waf/url-rewrite.url-rewrite-policy/rule?mkey=redirect-policy&sub_mkey={ruleId}";

            var response = await this.httpService.DeleteAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebRewritingPolicyUpdateData>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebRewritingPolicyUpdateData> DeleteRewritingRuleAsync(string ruleName)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/waf/url-rewrite.url-rewrite-rule?mkey={ruleName}";

            var response = await this.httpService.DeleteAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebRewritingPolicyUpdateData>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebIpMemberResult> GetIpListAsync(string name)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/waf/ip-list/members?mkey={name}";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebIpMemberResult>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebIpMemberCreateResult> CreateIpListAsync(string name, FortiWebIpMemberCreate data)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/waf/ip-list/members?mkey={name}";

            var response = await this.httpService.PostAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken), data);
            var result = JsonSerializer.Deserialize<FortiWebIpMemberCreateResult>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiWebIpMemberDeleteResult> RemoveIpListAsync(string name, FortiWebIpMember member)
        {
            var url = $"{Settings.FortiWebUrl}/api/v2.0/cmdb/waf/ip-list/members?mkey={name}&sub_mkey={member.Id}";

            var response = await this.httpService.DeleteAsync(url, HttpCredentials.WithAuthorization(Settings.FortiWebToken));
            var result = JsonSerializer.Deserialize<FortiWebIpMemberDeleteResult>(response);

            result.Json = response;

            return result;
        }
    }
}