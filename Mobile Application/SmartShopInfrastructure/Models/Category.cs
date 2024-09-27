using System.ComponentModel.DataAnnotations;

namespace SmartShopInfrastructure.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<Product> Products  { get; set; }
    }
}
