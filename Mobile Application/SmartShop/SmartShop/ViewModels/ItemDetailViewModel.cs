using MonkeyCache.FileStore;
using SmartShop.Models;
using SmartShop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartShop.ViewModels
{
    public class ItemDetailViewModel : BaseViewModel
    {
        private int productId;

        public ItemDetailViewModel(int id) : this()
        {
            productId = id;
        }
        public ItemDetailViewModel()
        {
            Products = new ObservableCollection<Product>();
            SelectedPhoto = new Command<Models.Image>(ChangePhoto);
            SwipeRightCommand = new Command<Models.Image>((image) => SwipePhoto(image, true));
            SwipeLeftCommand = new Command<Models.Image>((image) => SwipePhoto(image, false));
            ToggleProductInCartCommand = new Command<Product>(async (product) => await ToggleProductInCart(product));
            ToggleFavouriteProductCommand = new Command<Product>(async (product) => await ToggleProduct(product));
        }

        void SwipePhoto(Models.Image photo, bool directionRight)
        {
            if (Product.Images.Count <= 1)
                return;

            int currentPhotoIndex = Product.Images.FindIndex(s => s.Id == photo.Id);

            int nextPhotoIndex = directionRight ? currentPhotoIndex - 1 : currentPhotoIndex + 1;

            if (nextPhotoIndex < 0)
                nextPhotoIndex = Product.Images.Count - 1;

            if (nextPhotoIndex >= Product.Images.Count)
                nextPhotoIndex = 0;

            var nextPhoto = Product.Images[nextPhotoIndex];

            ChangePhoto(nextPhoto);
        }

        private void ChangePhoto(Models.Image photo)
        {
            Product.Image = photo;
        }

        public async void OnAppearing()
        {
            await LoadDataAsync();
        }

        public Product Product { get => _product; set => SetProperty(ref _product, value); }
        private Product _product;
        public ObservableCollection<Product> Products { get; set; }

        public Command<Models.Image> SelectedPhoto { get; }
        public ICommand ToggleProductInCartCommand { get; }
        public ICommand ToggleFavouriteProductCommand { get; }
        public Command<Models.Image> SwipeRightCommand { get; }
        public Command<Models.Image> SwipeLeftCommand { get; }

        public async Task LoadDataAsync()
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
                var productTask = ProductService.GetProductAsync(productId, SettingsService.AuthAccessToken);
                await Task.WhenAll(productTask, Task.Delay(1000));
                Product = await productTask;
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
                    State = Product.Id > 0 ? LayoutState.None : LayoutState.Empty;
                }
            }
        }

        private async Task ToggleProductInCart(Product product)
        {
            if(product == null)
            {
                return;
            }

            if (!VerifyInternetConnection())
            {
                return;
            }

            product.InCart = !product.InCart;

            try
            {
                if (IsLoggedIn())
                {
                    await CartService.ToggleProductInCartAsync(product.Id, SettingsService.AuthAccessToken);
                }
                else
                {
                    if (Barrel.Current.Exists(product.Id.ToString()))
                    {
                        Barrel.Current.Empty(product.Id.ToString());
                    }
                    else
                    {
                        Barrel.Current.Add(product.Id.ToString(), 1, TimeSpan.FromDays(90));
                    }
                }
            }
            catch (Exception ex)
            {
                product.InCart = !product.InCart;
                Debug.WriteLine(ex.Message);
            }
        }

        private async Task ToggleProduct(Product product)
        {
            if (!IsLoggedIn())
            {
                await OpenModalAsync(new LoginPage());
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
            await LoadDataAsync();
        }
    }
}
