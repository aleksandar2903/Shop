using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShop.Models
{
    public class Root<T> where T : class
    {
        [JsonProperty("current_page")]
        public int? CurrentPage { get; set; }
        [JsonProperty("total")]
        public int? Total { get; set; }
        [JsonProperty("per_page")]
        public int? PerPages { get; set; }
        [JsonProperty("to")]
        public int? To { get; set; }
        [JsonProperty("from")]
        public int? From { get; set; }
        [JsonProperty("last_page")]
        public int? LastPage { get; set; }
        [JsonProperty("data")]
        public List<T> Results { get; set; }

    }
}
