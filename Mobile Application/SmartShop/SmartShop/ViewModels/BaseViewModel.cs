using SmartShop.Models;
using SmartShop.Services;
using SmartShop.Services.Settings;
using SmartShop.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartShop.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IProductService ProductService => DependencyService.Get<ProductService>();
        public ICategoryBrandService CategoryBrandService => DependencyService.Get<CategoryBrandService>();
        public ISearchService SearchService => DependencyService.Get<SearchService>();
        public ICartService CartService => DependencyService.Get<CartService>();
        public IPromotionService PromotionService => DependencyService.Get<PromotionService>();
        public IAuthService AuthService => DependencyService.Get<IAuthService>();
        public ISettingsService SettingsService => DependencyService.Get<ISettingsService>();
        public IFavouriteService FavouriteService => DependencyService.Get<IFavouriteService>();
        public IOrderService OrderService => DependencyService.Get<IOrderService>();

        bool isBusy = false;
        LayoutState state = LayoutState.None;
        string customKeys;

        public ICommand BackwardCommand => new Command(async () => await Shell.Current.Navigation.PopAsync());
        public ICommand RefreshDataCommand => new Command(async () => await RefreshData());

        protected virtual Task RefreshData()
        {
            return Task.CompletedTask;
        }

        public ICommand SearchTappedCommand => new Command(async () => await Shell.Current.Navigation.PushAsync(new ExplorePage()));
        public ICommand UserTappedCommand => new Command(UserTapped);
        public ICommand ProductTappedCommand => new Command<Product>(async (product) => await Shell.Current.Navigation.PushModalAsync(new ItemDetailPage(product.Id)));

        public string CustomStateKey { get => customKeys; set => SetProperty(ref customKeys, value); }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public virtual Task InitializeAsync()
        {
            return Task.CompletedTask;
        }

        protected bool VerifyInternetConnection()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return false;
            }

            return true;
        }

        protected bool IsLoggedIn()
        {
            return !string.IsNullOrWhiteSpace(SettingsService.AuthAccessToken);
        }

        protected Task OpenModalAsync(Page page)
        {
             return App.Current.MainPage.Navigation.PushModalAsync(page);
        }

        public LayoutState State
        {
            get { return state; }
            set { SetProperty(ref state, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private async void UserTapped()
        {
            if (IsLoggedIn())
            {
                await Shell.Current.GoToAsync($"//{nameof(ProfilePage)}", true);
            }
            else
            {
                 await Shell.Current.GoToAsync(nameof(LoginPage), true);
            }
        }
    }
}
