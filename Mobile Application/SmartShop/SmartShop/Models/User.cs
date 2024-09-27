using Newtonsoft.Json;
using System;
using System.Linq;

namespace SmartShop.Models
{
    public class User
    {
        private string _name;
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get => _name; set { _name = value; Initials = value[0].ToString() ?? "-"; } }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("pending_orders_count")]
        public int PendingOrdersCount { get; set; }
        [JsonProperty("delivered_orders_count")]
        public int DeliveredOrdersCount { get; set; }
        [JsonProperty("client")]
        public Client Client { get; set; } = new Client();
        public string Initials { get; set; }
    }

    public class Client
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("document_id")]
        public int DocumentId { get; set; }
        [JsonProperty("total_paid")]
        public decimal TotalPaid { get; set; }
        [JsonProperty("last_purchase")]
        public DateTime LastPurchase { get; set; }
    }
}
