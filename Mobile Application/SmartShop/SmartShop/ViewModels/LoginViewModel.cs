using MonkeyCache.FileStore;
using SmartShop.Models.Request;
using SmartShop.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace SmartShop.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;
        private string _password;
        public Command LoginCommand { get; }
        public ICommand OpenRegisterPageCommand { get; }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await OnLoginClicked(), () => ValidateLoginRequest());
            OpenRegisterPageCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(RegisterPage)));
            this.PropertyChanged += (_, __) => LoginCommand.ChangeCanExecute();
        }

        private bool ValidateLoginRequest()
        {
            return !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password) && Password.Length >= 8;
        }

        public override async Task InitializeAsync()
        {
            if (IsLoggedIn())
            {
                await Shell.Current.Navigation.PopToRootAsync();
            }
        }

        private async Task JoinCartToUser()
        {
            var cart = await CartService.GetCartAsync(SettingsService.AuthAccessToken);
            var producstInCart = Barrel.Current.GetKeys();
            var tasks = new List<Task>();
            foreach (var productId in producstInCart)
            {
                if (Int32.TryParse(productId, out int id))
                {
                    int quantity = Barrel.Current.Get<int>(productId);
                    var current = cart.Where(c => c.Product.Id == id && c.Quantity != quantity).FirstOrDefault();
                    if (current is null)
                    {
                        tasks.Add(CartService.ToggleProductInCartAsync(id, SettingsService.AuthAccessToken, quantity));
                    }
                    else
                    {
                        await CartService.UpdateQuantity(current.Id, quantity, SettingsService.AuthAccessToken);
                    }
                }
            }
            var task = Task.WhenAll(tasks);
            await task;
        }
        private async Task OnLoginClicked()
        {
            if (State == LayoutState.Loading)
            {
                return;
            }

            if (!VerifyInternetConnection())
            {
                State = LayoutState.Custom;
                CustomStateKey = StateKeys.Offline;
                return;
            }

            State = LayoutState.Loading;

            try
            {
                var request = new LoginRequest
                {
                    Email = Email,
                    Password = Password,
                };

                var response = await AuthService.LogIn(request);

                if (response != null)
                {
                    SettingsService.AuthAccessToken = response.Token;
                    await JoinCartToUser();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                State = LayoutState.None;
                if (!IsLoggedIn())
                {
                    await Shell.Current.DisplayAlert("Greška", "Uneli ste neispravne podatke. Pokusajte ponovo", "U redu");
                }
                else
                {
                    await Shell.Current.Navigation.PopToRootAsync();
                }
            }
        }
    }
}
