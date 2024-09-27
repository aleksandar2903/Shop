using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartShop.Models
{
    public class Cart : INotifyPropertyChanged
    {
        public Cart()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonProperty("id")]
        public int Id { get; set; }
        int _quantity = 1;
        Product _product;

        public Cart(int quantity, int productId, decimal price, decimal amount)
        {
            Quantity = quantity;
            ProductId = productId;
            Price = price;
            Amount = amount;
        }

        [JsonProperty("quantity")]
        public int Quantity { get => _quantity; set { _quantity = value; Amount = value * Price; OnPropertyChanged(); OnPropertyChanged(nameof(Amount)); } }
        public Product Product
        {
            get => _product;
            set
            {
                _product = value;
                Price = value.Price;
                Amount = Quantity * value.Price;
            }
        }
        [JsonProperty("qty")]
        public int Qty { get => Quantity; set => Quantity = value;  }
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("product_id")]
        public int ProductId { get; set; }
        [JsonProperty("total_amount")]
        public decimal Amount { get; set; }
    }
}