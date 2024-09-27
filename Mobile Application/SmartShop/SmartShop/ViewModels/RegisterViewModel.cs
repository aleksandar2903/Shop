using MonkeyCache.FileStore;
using SmartShop.Models.Request;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace SmartShop.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _email;
        private string _password;
        private string _passwordConfirmation;
        private string _name;
        public Command RegisterCommand { get; }
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        public string PasswordConfirmation { get => _passwordConfirmation; set => SetProperty(ref _passwordConfirmation, value); }
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        public RegisterViewModel()
        {
            RegisterCommand = new Command(async () => await OnRegisterClicked(), () => ValidateRegisterRequest());
            this.PropertyChanged += (_, __) => RegisterCommand.ChangeCanExecute();
        }
        private bool ValidateRegisterRequest()
        {
            return !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password) && Password.Length >= 8 && !string.IsNullOrEmpty(Name) && Password == PasswordConfirmation;
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
        private async Task OnRegisterClicked()
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
                var request = new RegisterRequest
                {
                    Email = Email,
                    Password = Password,
                    Name = Name,
                    PasswordConfirmation = PasswordConfirmation
                };

                var response = await AuthService.Register(request);

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
