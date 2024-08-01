using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protyo.Utilities.Models.stripe
{
    public class Subscription
    {
        [JsonProperty("platformId")]
        public string platformId { get; set;}

        [JsonProperty("clientId")]
        public string clientId { get; set; }

        [JsonProperty("value")]
        public decimal value { get; set; }

    }
}

