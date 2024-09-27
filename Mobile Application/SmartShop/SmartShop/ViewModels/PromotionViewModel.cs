using SmartShop.Models;
using SmartShop.Views;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace SmartShop.ViewModels
{
    public class PromotionViewModel : BaseViewModel
    {
        Promotion promotion;
        public Promotion Promotion { get => promotion; set => SetProperty(ref promotion, value); }
        int id;
        public ICommand ToggleFavouriteProductCommand { get; }

        public PromotionViewModel()
        {
            ToggleFavouriteProductCommand = new Command<Product>(async (product) => await ToggleProduct(product));
        }

        public async void OnInitialize(int id)
        {
            if(id > 0)
            {
                this.id = id;
                await LoadDataAsync(id);
            }
        }

        public async Task LoadDataAsync(int id)
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
                var promotionTask = PromotionService.GetPromotion(id);
                await Task.WhenAll(promotionTask, Task.Delay(1000));
                Promotion = await promotionTask;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                State = LayoutState.Error;
            }
            finally
            {
                if (State != LayoutState.Error)
                {
                    State = Promotion.Products.Count > 0 ? LayoutState.None : LayoutState.Empty;
                }
            }
        }

        private async Task ToggleProduct(Product product)
        {
            if (!IsLoggedIn())
            {
                await Shell.Current.GoToAsync(nameof(LoginPage), true);
                return;
            }
            if (!VerifyInternetConnection())
            {
                return;
            }

            product.Favourite = !product.Favourite;

            try
            {
                await FavouriteService.ToogleFavourite(product.Id, SettingsService.AuthAccessToken);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                product.Favourite = !product.Favourite;
            }
        }

        protected override async Task RefreshData()
        {
            await LoadDataAsync(id);
        }
    }
}
