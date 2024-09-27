using SmartShop.Models;
using SmartShop.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace SmartShop.ViewModels
{
    public class FavouritesViewModel : BaseViewModel
    {
        public ObservableCollection<Product> FavouriteProducts { get; set; }
        public ICommand ToggleFavouriteProductCommand { get; }
        public ICommand OnProductTapped { get; }
        bool isLoggedIn;
        public FavouritesViewModel()
        {
            FavouriteProducts = new ObservableCollection<Product>();
            ToggleFavouriteProductCommand = new Command<Product>(async (product) => await ToggleProduct(product));
            OnProductTapped = new Command<Product>(OnProductSelected);
        }

        private async Task ToggleProduct(Product product)
        {
            if (product == null)
            {
                return;
            }

            try
            {
                await FavouriteService.ToogleFavourite(product.Id, SettingsService.AuthAccessToken);
                FavouriteProducts.Remove(product);
                if(FavouriteProducts.Count == 0)
                {
                    State = LayoutState.Empty;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                product.Favourite = false;
            }
        }

        public async override Task InitializeAsync()
        {
            State = LayoutState.Empty;
            await Task.Delay(1);

            if (!IsLoggedIn())
            {
                if (!isLoggedIn)
                {
                    isLoggedIn = true;
                    await Shell.Current.GoToAsync(nameof(LoginPage), true);
                }
            }
            else
            {
                await LoadDataAsync();
            }
        }

        private async Task LoadDataAsync()
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
                FavouriteProducts.Clear();

                var resultTask = FavouriteService.GetFavourites(SettingsService.AuthAccessToken);

                await Task.WhenAll(resultTask, Task.Delay(1000));

                var result = await resultTask;

                foreach (var product in result.Results)
                {
                    product.Favourite = true;
                    FavouriteProducts.Add(product);
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
                    State = FavouriteProducts.Count > 0 ? LayoutState.None : LayoutState.Empty;
                }
            }
        }

        async void OnProductSelected(Product product)
        {
            await Shell.Current.Navigation.PushModalAsync(new ItemDetailPage(product.Id));
        }

        protected override async Task RefreshData()
        {
            await LoadDataAsync();
        }
    }
}
