using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Protyo.DatabaseRefresh.Properties
{
    public static class HttpProperties
    {
       public static StringContent grantContent(int? days = null) =>
            new StringContent("{\r\n    \"keyword\": null,\r\n    \"oppNum\": null,\r\n    \"cfda\": null,\r\n    \"agencies\": null,\r\n    \"sortBy\": \"openDate|desc\",\r\n    \"rows\": 5000,\r\n    \"eligibilities\": null,\r\n    \"fundingCategories\": null,\r\n    \"fundingInstruments\": null,\r\n    \"dateRange\": \"\",\r\n    \"oppStatuses\": \"forecasted|posted\"\r\n}", null, "application/json");

        public static Dictionary<string, string> grantContentHeaders = new Dictionary<string, string> {
                    { "Accept", "application/json, text/plain, */*" },
                    { "Accept-Encoding", "gzip, deflate, br, zstd" },
                    { "Host", "apply07.grants.gov" }
            };

        public static Dictionary<string, string> grantDetailHeaders = new Dictionary<string, string> {
                    { "Accept", "*/*" },
                    { "Accept-Encoding", "gzip, deflate, br, zstd" },
                    { "Accept-Language", "en-US,en;q=0.9" },
                    { "Connection", "keep-alive" },
                    { "Host", "apply07.grants.gov" },
                    { "Origin", "https://www.grants.gov" },
                    { "Referer", "https://www.grants.gov/" },
                    { "Sec-Fetch-Mode", "cors" },
                    { "Sec-Fetch-Site", "same-site" },
                    { "User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/122.0.0.0 Safari/537.36" },
                    { "sec-ch-ua", "\"Chromium\";v=\"122\", \"Not(A:Brand\";v=\"24\", \"Google Chrome\";v=\"122\"" },
                    { "sec-ch-ua-mobile", "?0" },
                    { "sec-ch-ua-platform", "\"Windows\"" }

            };

        public static Dictionary<string, string> formGetHeaders(string token) => new Dictionary<string, string> {
                    { "Accept", "application/json, text/plain, */*" },
                    { "access-token", token }
            };
    }
}
