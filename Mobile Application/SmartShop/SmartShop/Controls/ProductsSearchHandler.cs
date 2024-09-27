using SmartShop.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductsSearchHandler : SearchHandler
    {
        public IList<Product> Products;
        public ProductsSearchHandler()
        {
            Products = new List<Product>
            {
                new Product
                {
                    Name = "Product 1"
                },
                new Product
                {
                    Name = "Product 1"
                },
                new Product
                {
                    Name = "Product 1"
                },
                new Product
                {
                    Name = "Product 1"
                }
            };
            ItemsSource = Products;
        }

        protected override void OnQueryChanged(string oldValue, string newValue)
        {
            base.OnQueryChanged(oldValue, newValue);

            Products.Clear();
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Product 1"
                },
                new Product
                {
                    Name = "Product 1"
                },
                new Product
                {
                    Name = "Product 1"
                },
                new Product
                {
                    Name = "Product 1"
                }
            };

            foreach (var product in products)
            {
                Products.Add(product);
            }
            ItemsSource = Products;
        }
    }
}