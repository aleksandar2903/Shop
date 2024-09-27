using SmartShop.Models;
using System.Threading.Tasks;

namespace SmartShop.Services
{
    public interface IFavouriteService
    {
        Task ToogleFavourite(int productId, string token);
        Task<Root<Product>> GetFavourites(string token);
    }
}
