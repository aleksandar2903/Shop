namespace SmartShopInfrastructure.Models
{
    public class OrderedProduct : BaseEntity
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public int Quantity { get; set; }
    }
}