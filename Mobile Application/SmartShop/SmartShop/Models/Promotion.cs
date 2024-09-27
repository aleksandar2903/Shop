using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShop.Models
{
    public class Promotion
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("image")]
        public string ImageName { get; set; }
        public Uri ImageUrl { get => new Uri($"{Config.StorageAddress}{ImageName}"); }

        [JsonProperty("products")]
        public List<Product> Products { get; set; }
    }
}
