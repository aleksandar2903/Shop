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
    public class ProfileViewModel : BaseViewModel
    {
        private User _user;
        private bool _isLoginPageOpened;
        public User User { get => _user; set => SetProperty(ref _user, value); }
        public ICommand LogoutCommand { get; }
        public ICommand PurchaseHistoryCommand { get; }
        public ICommand FavouriteCommand { get; }

        public ProfileViewModel()
        {
            LogoutCommand = new Command(async () => { SettingsService.AuthAccessToken = String.Empty; await RefreshData(); });
            PurchaseHistoryCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(OrderPage), true));
            FavouriteCommand = new Command(async () => await Shell.Current.GoToAsync($"//{nameof(FavouritesPage)}", true));
        }

        public override async Task InitializeAsync()
        {
            if(User is null)
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

            if (!IsLoggedIn())
            {
                await Task.Delay(600);

                State = LayoutState.Empty;

                if (_isLoginPageOpened)
                {
                    return;
                }
                await Shell.Current.GoToAsync(nameof(LoginPage), true);
                _isLoginPageOpened = true;
                return;
            }
            try
            {
                var userTask = AuthService.GetUserAsync(SettingsService.AuthAccessToken);

                await Task.WhenAll(userTask, Task.Delay(1000));

                User = await userTask;
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
                    State = User.Id > 0 ? LayoutState.None : LayoutState.Empty;
                }
            }
        }

        protected override async Task RefreshData()
        {
            User = null;
            await LoadDataAsync();
        }
    }
}
