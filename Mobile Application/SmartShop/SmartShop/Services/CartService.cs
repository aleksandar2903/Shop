using SmartShop.Models;
using SmartShop.Services.RequestProvider;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartShop.Services
{
    public class CartService : ICartService
    {
        private readonly string url = $"{Config.APIUrl}/cart";
        private readonly IRequestProvider _requestProvider;
        public CartService()
        {
            _requestProvider = DependencyService.Get<IRequestProvider>();
        }

        public async Task<IEnumerable<Cart>> GetCartAsync(string token)
        {
            var catalog = await _requestProvider.GetAsync<IEnumerable<Cart>>(url, token).ConfigureAwait(false);

            return catalog ?? Enumerable.Empty<Cart>();
        }

        public async Task ToggleProductInCartAsync(int productId, string token, int quantity = 1)
        {
            var data = new
            {
                product_id = productId,
                quantity = quantity,
            };
            await _requestProvider.PostAsync<object, object>(url, data, token).ConfigureAwait(false);
        }

        public async Task UpdateQuantity(int cartId, int quantity, string token)
        {
            var data = new
            {
                quantity = quantity,
            };
            await _requestProvider.PutAsync<object, object>($"{url}/{cartId}", data, token).ConfigureAwait(false);
        }
    }
}
