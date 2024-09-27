using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShop.Models
{
    public class FilterResponse
    {
        [JsonProperty("categories")]
        public List<Subcategory> Categories { get; set; }
        [JsonProperty("brands")]
        public List<Brand> Brands { get; set; }
        [JsonProperty("max_product_price")]
        public decimal? MaxProductPrice { get; set; }
        [JsonProperty("min_product_price")]
        public decimal? MinProductPrice { get; set; }
        [JsonProperty("total_records")]
        public int TotalRecords { get; set; }
    }
}
