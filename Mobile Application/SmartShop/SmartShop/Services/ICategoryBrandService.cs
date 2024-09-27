using SmartShop.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartShop.Services
{
    public interface ICategoryBrandService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
        Task<IEnumerable<Subcategory>> GetSubcategoriesAsync(int categoryId);
        Task<IEnumerable<Brand>> GetBrandsAsync();
    }
}
