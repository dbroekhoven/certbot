using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Cre8ion.Common;

namespace Shared.Services
{
    public class HttpService : IService
    {
        public async Task<string> GetAsync(string url)
        {
            return await this.GetResponseAsync(HttpMethod.Get, url, null, null);
        }

        public async Task<string> GetAsync(string url, HttpCredentials credentials)
        {
            return await this.GetResponseAsync(HttpMethod.Get, url, credentials, null);
        }

        public async Task<string> PostAsync(string url, object model)
        {
            return await this.GetResponseAsync(HttpMethod.Post, url, null, model);
        }

        public async Task<string> PostAsync(string url, HttpCredentials credentials, object model)
        {
            return await this.GetResponseAsync(HttpMethod.Post, url, credentials, model, null);
        }

        public async Task<string> PutAsync(string url, HttpCredentials credentials, object model)
        {
            return await this.GetResponseAsync(HttpMethod.Put, url, credentials, model, null);
        }

        public async Task<string> DeleteAsync(string url, HttpCredentials credentials)
        {
            return await this.GetResponseAsync(HttpMethod.Delete, url, credentials, null, null);
        }

        public async Task<string> UploadAsync(string url, HttpCredentials credentials, MultipartFormDataContent upload)
        {
            return await this.GetResponseAsync(HttpMethod.Patch, url, credentials, null, upload);
        }

        public async Task<T> GetJsonAsync<T>(string url, HttpCredentials credentials)
        {
            using var httpHandler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    return true;
                }
            };

            using var client = new HttpClient(httpHandler);
            client.DefaultRequestHeaders.Add("User-Agent", "The Cre8ion.Lab - CertBot");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var uri = new Uri(url);

            if (credentials != null && !string.IsNullOrEmpty(credentials.Authorization))
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", credentials.Authorization);
            }
            else if (credentials != null && !string.IsNullOrEmpty(credentials.AccessToken))
            {
                var uriBuilder = new UriBuilder(url);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["access_token"] = credentials.AccessToken;
                uriBuilder.Query = query.ToString();

                uri = uriBuilder.Uri;
            }

            var response = await client.GetAsync(uri);
            var result = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(result);
        }

        private async Task<string> GetResponseAsync(HttpMethod method, string url, HttpCredentials credentials, object model = null, MultipartFormDataContent upload = null)
        {
            using var httpHandler = new HttpClientHandler
            {
                SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
                {
                    return true;
                }
            };

            using var client = new HttpClient(httpHandler);
            client.DefaultRequestHeaders.Add("User-Agent", "The Cre8ion.Lab - CertBot");
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            var uri = new Uri(url);

            if (credentials != null && !string.IsNullOrEmpty(credentials.Authorization))
            {
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", credentials.Authorization);
            }
            else if (credentials != null && !string.IsNullOrEmpty(credentials.AccessToken))
            {
                var uriBuilder = new UriBuilder(url);
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["access_token"] = credentials.AccessToken;
                uriBuilder.Query = query.ToString();

                uri = uriBuilder.Uri;
            }

            try
            {
                var response = default(HttpResponseMessage);

                if (method == HttpMethod.Get)
                {
                    response = await client.GetAsync(uri);
                }
                else if (method == HttpMethod.Delete)
                {
                    response = await client.DeleteAsync(uri);
                }
                else if (method == HttpMethod.Post)
                {
                    var json = JsonSerializer.Serialize(model);
                    using var content = new StringContent(json, Encoding.UTF8, "application/json");

                    response = await client.PostAsync(uri, content);
                }
                else if (method == HttpMethod.Patch)
                {
                    response = await client.PostAsync(uri, upload);
                }
                else if (method == HttpMethod.Put)
                {
                    var json = JsonSerializer.Serialize(model);
                    using var content = new StringContent(json, Encoding.UTF8, "application/json");

                    response = await client.PutAsync(uri, content);
                }

                if (response == null)
                {
                    return string.Empty;
                }

                var result = await response.Content.ReadAsStringAsync();

                response.Dispose();

                return result;
            }
            catch (HttpRequestException exception)
            {
                return exception.Message;
            }
            catch (WebException exception)
            {
                return exception.Message;
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}