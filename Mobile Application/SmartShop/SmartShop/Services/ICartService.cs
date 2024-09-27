using SmartShop.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartShop.Services
{
    public interface ICartService
    {
        Task ToggleProductInCartAsync(int productId, string token, int quantity = 1);
        Task UpdateQuantity(int cartId, int quantity, string token);
        Task<IEnumerable<Cart>> GetCartAsync(string token);
    }
}
