using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShop.Models
{
    public class Subcategory
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("product_category_id")]
        public int CategoryId { get; set; }
        [JsonProperty("products_count")]
        public int ProductsCount { get; set; }
        [JsonProperty("is_selected")]
        public bool IsSelected { get; set; }

        [JsonProperty("products")]
        public List<Product> Products { get; set; }
    }
}
