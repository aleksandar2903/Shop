using SmartShop.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartShop.Services
{
    public interface IProductService
    {
        Task<Product> GetProductAsync(int id, string token = "");
        Task<IEnumerable<Product>> GetPopularProductsAsync(string token = "");
        Task<IEnumerable<Product>> GetNewestProductsAsync(string token = "");
        Task<IEnumerable<Product>> GetBulkProductsAsync(string ids);
    }
}
