using SmartShop.Models;
using System.Threading.Tasks;
using SmartShop.Services.RequestProvider;
using Xamarin.Forms;

namespace SmartShop.Services
{
    public class SearchService : ISearchService
    {
        private readonly IRequestProvider _requestProvider;
        public SearchService()
        {
            _requestProvider = DependencyService.Get<IRequestProvider>();
        }
        public async Task<Root<Product>> SearchProducts(string query, string categories = "", string brands = "", decimal priceMin = 0, decimal priceMax = 0, string sortBy = "", int page = 1, int mainCategory = 0, string token = "")
        {
            var catalog = await _requestProvider.GetAsync<Root<Product>>($"{Config.APIUrl}{(mainCategory > 0 ? $"/category/{mainCategory}/" : "/")}search?page={page}&query={query}&categories={categories}&brands={brands}&priceMin={priceMin}&priceMax={priceMax}&sortBy={sortBy}", token).ConfigureAwait(false);

            return catalog ?? new Root<Product>();
        }

        public async Task<FilterResponse> FilterProducts(string query, string categories = "", string brands = "", decimal priceMin = 0, decimal priceMax = 0, int mainCategory = 0)
        {
            var catalog = await _requestProvider.GetAsync<FilterResponse>($"{Config.APIUrl}{(mainCategory > 0 ? $"/category/{mainCategory}/" : "/")}filter?query={query}&categories={categories}&brands={brands}&priceMin={priceMin}&priceMax={priceMax}").ConfigureAwait(false);

            return catalog ?? new FilterResponse();
        }
    }
}
