using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace INachalnickUtilities.Helpers
{
    public class HttpHelper
    {
        public Dictionary<string, string>? DefaultHeaders { get; set; } = null;
        public string? BaseUrl { get; set; } = null;
        static HttpHelper()
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.Expect100Continue = false;
        }
        private HttpClient GetClient(Dictionary<string, string>? headers = null)
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            // Pass the handler to httpclient(from you are calling api)
            HttpClient client = new HttpClient(clientHandler);
            SetHeaders(client, DefaultHeaders);
            SetHeaders(client, headers);
            if (!string.IsNullOrEmpty(BaseUrl))
            {
                client.BaseAddress = new System.Uri(BaseUrl);
            }
            return client;
        }
        private void SetHeaders(HttpClient client, Dictionary<string, string>? headers)
        {
            headers ??= new Dictionary<string, string>();
            foreach (var key in headers.Keys)
            {
                client.DefaultRequestHeaders.Add(key, headers[key]);
            }
        }

        public Task<HttpResponseMessage> Delete(string url, dynamic? data = null, Dictionary<string, string>? headers = null)
        {
            return Method("DELETE", url, data, headers);
        }

        public async Task<HttpResponseMessage> Post(string url, dynamic? data = null, Dictionary<string, string>? headers = null)
        {
            using var client = GetClient(headers);
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            return await client.PostAsync(url, content);
        }

        public async Task<HttpResponseMessage> Patch(string url, dynamic? data = null, Dictionary<string, string>? headers = null)
        {
            using var client = GetClient(headers);
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            return await client.PatchAsync(url, content);
        }

        public async Task<HttpResponseMessage> Get(string url, Dictionary<string, string>? headers = null)
        {
            using var client = GetClient(headers);
            return await client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> Method(string method, string url, dynamic? data = null, Dictionary<string, string>? headers = null)
        {
            using var client = GetClient(headers);
            string body = JsonConvert.SerializeObject(data, Formatting.None);
            var content = new StringContent(body);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            url = url.ToLower().Contains("http") ? url : BaseUrl + url;
            return await client.SendAsync(new HttpRequestMessage
            {
                Method = new HttpMethod(method),
                Content = content,
                RequestUri = new System.Uri(url),
            });
        }
    }
}