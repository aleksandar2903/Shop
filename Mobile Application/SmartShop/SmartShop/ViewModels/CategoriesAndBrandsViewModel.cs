using SmartShop.Models;
using SmartShop.Services;
using SmartShop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace SmartShop.ViewModels
{
    public class CategoriesAndBrandsViewModel : BaseViewModel
    {
        public ObservableCollection<Category> Categories { get; }
        public ObservableCollection<Brand> Brands { get; }
        public Command ForwardCommand { get; }
        public Command OnBrandSelectCommand { get; }
        public CategoriesAndBrandsViewModel()
        {
            Categories = new ObservableCollection<Category>();
            Brands = new ObservableCollection<Brand>();
            ForwardCommand = new Command<Category>(async (category) => await Shell.Current.Navigation.PushAsync(new SubcategoriesPage(category)));
            OnBrandSelectCommand = new Command<Brand>(async (brand) => await Shell.Current.Navigation.PushAsync(new ExplorePage(brand: brand.Id.ToString())));
        }

        public async void OnAppearing()
        {
            if(Categories.Count == 0 || Brands.Count == 0)
                await LoadDataAsync();
        }

        async Task LoadDataAsync()
        {
            if (!VerifyInternetConnection())
            {
                State = LayoutState.Custom;
                CustomStateKey = StateKeys.Offline;
                return;
            }

            State = LayoutState.Loading;

            try
            {
                Categories.Clear();
                Brands.Clear();

                var categoriesTask = CategoryBrandService.GetCategoriesAsync();

                var brandsTask = CategoryBrandService.GetBrandsAsync();

                await Task.WhenAll(categoriesTask, brandsTask, Task.Delay(1000));

                var categories = await categoriesTask;
                var brands = await brandsTask;

                foreach (var category in categories)
                {
                    Categories.Add(category);
                }


                foreach (var brand in brands)
                {
                    Brands.Add(brand);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                State = LayoutState.Error;
            }
            finally
            {
                if (State != LayoutState.Error)
                {
                    State = Categories.Count > 0 && Brands.Count > 0 ? LayoutState.None : LayoutState.Empty;
                }
            }
        }

        protected override async Task RefreshData()
        {
            await LoadDataAsync();
        }
    }
}
