using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShop.Models
{
    public class Image
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("w500")]
        public string W500Size { get; set; }
        [JsonProperty("original")]
        public string OriginalSize { get; set; }
        public Uri Uri { get => new Uri($"{Config.StorageAddress}{W500Size}"); }
    }
}
