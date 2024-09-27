using SmartShop.Models;
using SmartShop.Services.RequestProvider;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartShop.Services
{
    public class FavouriteService : IFavouriteService
    {
        private readonly string url = $"{Config.APIUrl}/favourites";
        private readonly IRequestProvider _requestProvider;
        public FavouriteService()
        {
            _requestProvider = DependencyService.Get<IRequestProvider>();
        }
        public async Task<Root<Product>> GetFavourites(string token)
        {
            var catalog = await _requestProvider.GetAsync<Root<Product>>(url, token).ConfigureAwait(false);

            return catalog ?? new Root<Product>();
        }

        public async Task ToogleFavourite(int productId, string token)
        {
            var data = new
            {
                product_id = productId,
            };
            await _requestProvider.PostAsync<object, object>(url, data, token).ConfigureAwait(false);
        }
    }
}
