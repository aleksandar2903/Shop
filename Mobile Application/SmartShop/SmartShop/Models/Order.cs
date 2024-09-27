using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SmartShop.Models
{
    public class Order
    {
        private string deliveryStatus = string.Empty;
        private string paidStatus = string.Empty;
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("shipping_address")]
        public ShippingAddress ShippingAddress { get; set; }
        [JsonProperty("delivery_status")]
        public string DeliveryStatus {
            get
            {
                var status = deliveryStatus;

                switch (deliveryStatus)
                {
                    case "Pending":
                        status = "Na čekanju";
                        break;
                    case "On Delivery":
                        status = "Poslat na isporuku";
                        break;
                    case "Delivered":
                        status = "Isporučen";
                        break;
                    case "Ready for Delivery":
                        status = "Spreman za isporuku";
                        break;
                };

                return status;
            }
            set { deliveryStatus = value; }
        }
        [JsonProperty("status")]
        public string PaidStatus {
            get
            {
                var status = paidStatus;

                switch (paidStatus)
                {
                    case "Paid":
                        status = "Plaćen";
                        break;
                    case "Unpaid":
                        status = "Neplaćen";
                        break;
                };

                return status;
            }
            set { paidStatus = value; }
        }
        [JsonProperty("products")]
        public List<Cart> Products { get; set; }
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount => Products.Sum(p => p.Amount);
    }
}
