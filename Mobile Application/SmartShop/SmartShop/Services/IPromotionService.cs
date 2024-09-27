using SmartShop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartShop.Services
{
    public interface IPromotionService
    {
        Task<IEnumerable<Promotion>>GetPromotions();
        Task<Promotion>GetPromotion(int promotionId, string token = "");
    }
}
