using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SmartShop.Models
{
    public class Product : INotifyPropertyChanged
    {
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
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("rating_count")]
        public int RatingCount { get; set; }
        [JsonProperty("rating")]
        public double Rating { get; set; }
        public string RatingStars
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();

                if (Rating > 0)
                {
                    int ratingCell = (int)Rating;
                    int remainder = 5 - ratingCell;

                    if (ratingCell > 0)
                        stringBuilder.Insert(0, "★", ratingCell);

                    if (remainder > 0)
                        stringBuilder.Insert(ratingCell, "☆", remainder);

                }

                return stringBuilder.ToString();
            }
        }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("image")]
        public Image Image { get => _image; set { _image = value; OnPropertyChanged(); } }
        Image _image;
        bool _favourite;
        bool _inCart;
        [JsonProperty("price")]
        public decimal Price { get; set; }
        [JsonProperty("brand")]
        public Brand Brand { get; set; }
        [JsonProperty("favourite")]
        public bool Favourite { get => _favourite; set { _favourite = value; OnPropertyChanged(); } }
        [JsonProperty("in_cart")]
        public bool InCart { get => _inCart; set { _inCart = value; OnPropertyChanged(); } }
        [JsonProperty("similar_product")]
        public Product SimilarProduct { get; set; }
        [JsonProperty("similar_products")]
        public List<Product> SimilarProducts { get; set; }
        [JsonProperty("popular_brands")]
        public List<Product> PopularProducts { get; set; }
        [JsonProperty("stock")]
        public int Quantity { get; set; }
        [JsonProperty("subcategory_with_category")]
        public Subcategory Subcategory { get; set; }
        [JsonProperty("product_subcategory_id")]
        public int SubcategoryId { get; set; }
        [JsonProperty("images")]
        public List<Image> Images { get; set; }
        [JsonProperty("published_reviews")]
        public List<Review> Reviews { get; set; }
        [JsonProperty("specification_attributes")]
        public List<AttributeValue> SpecificationAttributes { get; set; }
        RatingStars _ratingStars;
        public RatingStars RatingStar { get => _ratingStars; set => _ratingStars.Value = (int)Rating; }
    }
}
