using SmartShop.Models;
using SmartShop.Services.RequestProvider;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartShop.Services
{
    public class ProductService : IProductService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly string url = $"{Config.APIUrl}/products";

        public ProductService()
        {
            _requestProvider = DependencyService.Get<IRequestProvider>();
        }
        public async Task<IEnumerable<Product>> GetPopularProductsAsync(string token = "")
        {
            var catalog = await _requestProvider.GetAsync<Root<Product>>($"{url}/popular", token).ConfigureAwait(false);

            return catalog?.Results ?? Enumerable.Empty<Product>();
        }

        public async Task<IEnumerable<Product>> GetNewestProductsAsync(string token = "")
        {
            var catalog = await _requestProvider.GetAsync<Root<Product>>($"{url}/newest", token).ConfigureAwait(false);

            return catalog?.Results ?? Enumerable.Empty<Product>();
        }

        public async Task<Product> GetProductAsync(int id, string token = "")
        {
            var product = await _requestProvider.GetAsync<Product>($"{url}/{id}", token).ConfigureAwait(false);

            return product ?? new Product();
        }

        public async Task<IEnumerable<Product>> GetBulkProductsAsync(string ids)
        {
            var catalog = await _requestProvider.GetAsync<IEnumerable<Product>>($"{url}/bulk?ids={ids}").ConfigureAwait(false);

            return catalog ?? Enumerable.Empty<Product>();
        }
    }
}