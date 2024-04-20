using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Protyo.Utilities.Services.Contracts
{
    public interface IHttpService
    {
        public HttpService Initialize(string baseUrl, HttpMethod method);
        public HttpService AddHeaders(Dictionary<string,string> httpHeaders);
        public HttpService AddContent(StringContent content);
        public Task<HttpResponseMessage> SendRequest();
    }
}
