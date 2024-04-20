using Protyo.Utilities.Services.Contracts;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Protyo.Utilities.Services
{
    public class HttpService : IHttpService
    {
        private HttpClient Client;

        private HttpRequestMessage Request;

        public HttpService(HttpClient client) {
            Client = client;
        }

        public HttpService Initialize(string baseUrl, HttpMethod method) {
            Request = new HttpRequestMessage(method, baseUrl);
            return this;
        }

        public HttpService AddHeaders(Dictionary<string, string> httpHeaders)
        {
            foreach (var header in httpHeaders)
                Request.Headers.Add(header.Key, header.Value);
            return this;
        }

        public HttpService AddContent(StringContent content){ Request.Content = content; return this;}
      
        public HttpService AddContent(List<KeyValuePair<string,string>> content) { Request.Content = new FormUrlEncodedContent(content); return this; }

        public async Task<HttpResponseMessage> SendRequest() => await Client.SendAsync(Request);


    }
}
