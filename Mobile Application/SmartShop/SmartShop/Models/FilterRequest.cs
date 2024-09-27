namespace SmartShop.Models
{
    public class FilterRequest
    {
        public string Categories { get; set; }
        public string Brands { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public string SortBy { get; set; }
    }
}
