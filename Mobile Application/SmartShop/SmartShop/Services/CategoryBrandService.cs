using SmartShop.Models;
using SmartShop.Services.RequestProvider;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SmartShop.Services
{
    public class CategoryBrandService : ICategoryBrandService
    {
        private readonly IRequestProvider _requestProvider;
        private readonly string urlBrands = $"{Config.APIUrl}/brands";
        private readonly string urlCategories = $"{Config.APIUrl}/categories";

        public CategoryBrandService()
        {
            _requestProvider = DependencyService.Get<IRequestProvider>();
        }
        public async Task<IEnumerable<Brand>> GetBrandsAsync()
        {
            var brands = await _requestProvider.GetAsync<IEnumerable<Brand>>($"{urlBrands}").ConfigureAwait(false);

            return brands ?? Enumerable.Empty<Brand>();
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            var categories = await _requestProvider.GetAsync<IEnumerable<Category>>($"{urlCategories}").ConfigureAwait(false);

            return categories ?? Enumerable.Empty<Category>();
        }

        public async Task<IEnumerable<Subcategory>> GetSubcategoriesAsync(int categoryId)
        {
            var subcategories = await _requestProvider.GetAsync<IEnumerable<Subcategory>>($"{urlCategories}/{categoryId}/subcategories").ConfigureAwait(false);

            return subcategories ?? Enumerable.Empty<Subcategory>();
        }
    }
}
