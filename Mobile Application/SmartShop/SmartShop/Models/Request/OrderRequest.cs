using Newtonsoft.Json;
using System.Collections.Generic;

namespace SmartShop.Models.Request
{
    public class OrderRequest
    {
        public OrderRequest()
        {

        }
        public OrderRequest(decimal totalAmount, ShippingAddress shippingAddress, int paymentMethodId, List<Cart> products)
        {
            TotalAmount = totalAmount;
            ShippingAddress = shippingAddress;
            PaymentMethodId = paymentMethodId;
            Products = products;
        }

        [JsonProperty("total_amount")]
        public decimal TotalAmount { get; set; }
        [JsonProperty("shipping_address")]
        public ShippingAddress ShippingAddress { get; set; }
        [JsonProperty("payment_method_id")]
        public int PaymentMethodId { get; set; }
        [JsonProperty("products")]
        public List<Cart> Products { get; set; }
    }
}
