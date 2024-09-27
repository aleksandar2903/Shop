using Newtonsoft.Json;

namespace SmartShop.Models
{
    public class AttributeValue
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("value")]
        public string Value { get; set; }
        [JsonProperty("attribute")]
        public SpecificationAttribute SpecificationAttribute { get; set; }
    }
}
