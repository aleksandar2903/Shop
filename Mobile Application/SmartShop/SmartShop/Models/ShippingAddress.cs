using Newtonsoft.Json;

namespace SmartShop.Models
{
    public class ShippingAddress
    {
        public ShippingAddress(string name, string email, string phone, string address, string city, string zipCode)
        {
            Name = name;
            Email = email;
            Phone = phone;
            Address = address;
            City = city;
            ZipCode = zipCode;
        }

        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("address")]
        public string Address { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("zip")]
        public string ZipCode { get; set; }
    }
}
