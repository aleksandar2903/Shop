using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShop.Models
{
    public class Category
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        public Uri Img { get => new Uri($"{Config.StorageAddress}{Image}"); }

        [JsonProperty("subcategories")]
        public List<Subcategory> Subcategories { get; set; }
    }
}
