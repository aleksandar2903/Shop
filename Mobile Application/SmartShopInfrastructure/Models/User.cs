using Microsoft.AspNetCore.Identity;

namespace SmartShopInfrastructure.Models
{
    public class User : IdentityUser
    {
        public virtual List<Order> Orders { get; set; }
    }
}
