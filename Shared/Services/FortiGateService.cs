using System.Text.Json;
using System.Threading.Tasks;
using Cre8ion.Common;
using Shared.FortiGateModels;

namespace Shared.Services
{
    public class FortiGateService : IService
    {
        private readonly HttpService httpService;

        private readonly string url = "https://fortigate.intern.cre8ion.nl";
        private readonly string token = "zGm6hfk9pNpmhb79ct818Hgj74gQjq";

        public FortiGateService(HttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<FortiGateStatus> GetStatusAsync()
        {
            var url = $"{this.url}/api/v2/monitor/license/status";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAccessToken(this.token));
            var result = JsonSerializer.Deserialize<FortiGateStatus>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiGateAddressList> GetIpv4AddressListAsync()
        {
            var url = $"{this.url}/api/v2/cmdb/firewall/address?with_meta=1&datasource=1&exclude-default-values=1&start=0&count=500&vdom=root";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAccessToken(this.token));
            var result = JsonSerializer.Deserialize<FortiGateAddressList>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiGatePostResult> CreateIpv4AddressAsync(FortiGateAddressCreate data)
        {
            var url = $"{this.url}/api/v2/cmdb/firewall/address?datasource=1&vdom=root";

            var response = await this.httpService.PostAsync(url, HttpCredentials.WithAccessToken(this.token), data);
            var result = JsonSerializer.Deserialize<FortiGatePostResult>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiGatePostResult> DeleteIpv4AddressAsync(string name)
        {
            var url = $"{this.url}/api/v2/cmdb/firewall/address/{name}?vdom=root";

            var response = await this.httpService.DeleteAsync(url, HttpCredentials.WithAccessToken(this.token));
            var result = JsonSerializer.Deserialize<FortiGatePostResult>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiGatePostResult> UpdateIpv4AddressListAsync(string name, FortiGateAddressListUpdate data)
        {
            var url = $"{this.url}/api/v2/cmdb/firewall/addrgrp/{name}?datasource=1&vdom=root";

            var response = await this.httpService.PutAsync(url, HttpCredentials.WithAccessToken(this.token), data);
            var result = JsonSerializer.Deserialize<FortiGatePostResult>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiGateAddressList> GetIpv6AddressListAsync()
        {
            var url = $"{this.url}/api/v2/cmdb/firewall/address6?with_meta=1&datasource=1&exclude-default-values=1&start=0&count=24&vdom=root";

            var response = await this.httpService.GetAsync(url, HttpCredentials.WithAccessToken(this.token));
            var result = JsonSerializer.Deserialize<FortiGateAddressList>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiGatePostResult> CreateIpv6AddressAsync(FortiGateAddressCreate data)
        {
            var url = $"{this.url}/api/v2/cmdb/firewall/address6?datasource=1&vdom=root";

            var response = await this.httpService.PostAsync(url, HttpCredentials.WithAccessToken(this.token), data);
            var result = JsonSerializer.Deserialize<FortiGatePostResult>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiGatePostResult> DeleteIpv6AddressAsync(string name)
        {
            var url = $"{this.url}/api/v2/cmdb/firewall/address6/{name}?vdom=root";

            var response = await this.httpService.DeleteAsync(url, HttpCredentials.WithAccessToken(this.token));
            var result = JsonSerializer.Deserialize<FortiGatePostResult>(response);

            result.Json = response;

            return result;
        }

        public async Task<FortiGatePostResult> UpdateIpv6AddressListAsync(string name, FortiGateAddressListUpdate data)
        {
            var url = $"{this.url}/api/v2/cmdb/firewall/addrgrp6/{name}?datasource=1&vdom=root";

            var response = await this.httpService.PutAsync(url, HttpCredentials.WithAccessToken(this.token), data);
            var result = JsonSerializer.Deserialize<FortiGatePostResult>(response);

            result.Json = response;

            return result;
        }
    }
}