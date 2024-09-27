namespace SmartShopInfrastructure.Models
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public bool IsAccepted { get; set; }
        public virtual List<OrderedProduct> OrderedProducts { get; set; }
    }
}
