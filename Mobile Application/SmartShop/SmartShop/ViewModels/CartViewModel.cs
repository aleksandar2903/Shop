using MonkeyCache.FileStore;
using SmartShop.Models;
using SmartShop.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace SmartShop.ViewModels
{
    public class CartViewModel : BaseViewModel
    {
        private decimal _totalAmount = 0;
        public ObservableCollection<Cart> Cart { get; set; }
        public ICommand ToggleProductCommand { get; }
        public ICommand DecreaseProductQuatityCommand { get; }
        public ICommand CheckoutCommand { get; }
        public ICommand IncreaseProductQuatityCommand { get; }
        public ICommand EnterProductQuatityCommand { get; }
        private CancellationTokenSource cts = new CancellationTokenSource();
        private DateTime timerStarted { get; set; } = DateTime.UtcNow.AddYears(-1);
        public decimal TotalAmount { get => _totalAmount; set => SetProperty(ref _totalAmount, value); }

        public CartViewModel()
        {
            Cart = new ObservableCollection<Cart>();
            CheckoutCommand = new Command(Checkout);
            ToggleProductCommand = new Command<Cart>(async (cart) => await ToggleProductInCart(cart));
            DecreaseProductQuatityCommand = new Command<Cart>((cart) => { if (cart.Quantity > 0) { cart.Quantity--; Throttle(500, async _ => await UpdateQuantity(cart)); } });
            IncreaseProductQuatityCommand = new Command<Cart>((cart) => { if (cart.Quantity < cart.Product.Quantity) { cart.Quantity++; Throttle(500, async _ => await UpdateQuantity(cart)); } });
            EnterProductQuatityCommand = new Command<Cart>((cart) => { if (cart.Quantity <= cart.Product.Quantity) { Throttle(500, async _ => await UpdateQuantity(cart)); } });
        }

        private async void Checkout()
        {
            if (IsLoggedIn())
            {
                await Shell.Current.Navigation.PushAsync(new ShippingPage(), true);
            }
            else
            {
                await Shell.Current.Navigation.PushModalAsync(new LoginPage(), true);
            }
        }

        private async Task ToggleProductInCart(Cart cart)
        {
            if (cart == null)
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
                if (IsLoggedIn())
                {
                    await CartService.ToggleProductInCartAsync(cart.Product.Id, SettingsService.AuthAccessToken);
                }
                else
                {
                    Barrel.Current.Empty(cart.Product.Id.ToString());
                }

                Cart.Remove(cart);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                UpdateTotalAmount();
                State = Cart.Count > 0 ? LayoutState.None : LayoutState.Empty;
            }
        }

        public async void OnAppearing()
        {
            await LoadDataAsync();
        }

        protected override async Task RefreshData()
        {
            await LoadDataAsync();
        }

        private async Task UpdateQuantity(Cart cart)
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
                if (cart.Quantity > 0)
                {
                    if (IsLoggedIn())
                    {
                        await CartService.UpdateQuantity(cart.Id, cart.Quantity, SettingsService.AuthAccessToken);
                    }
                    else
                    {
                        Barrel.Current.Empty(cart.Product.Id.ToString());
                        Barrel.Current.Add<int>(cart.Product.Id.ToString(), cart.Quantity, TimeSpan.FromDays(90));
                    }
                    UpdateTotalAmount();
                }
                else
                {
                    await ToggleProductInCart(cart);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                State = LayoutState.Error;
            }
            finally
            {
                if (State != LayoutState.Error && State != LayoutState.Empty)
                {
                    State = LayoutState.None;
                }
            }
        }
        void UpdateTotalAmount()
        {
            TotalAmount = Cart.Sum(s => s.Amount);
        }
        private void Throttle(int interval, Action<object> action, object param = null)
        {
            cts.Cancel();
            cts = new CancellationTokenSource();

            var curTime = DateTime.UtcNow;
            if (curTime.Subtract(timerStarted).TotalMilliseconds < interval)
                interval -= (int)curTime.Subtract(timerStarted).TotalMilliseconds;

            Task.Run(async delegate
            {
                await Task.Delay(interval, cts.Token);
                action.Invoke(param);
            });

            timerStarted = curTime;
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
                Cart.Clear();
                TotalAmount = 0;

                if (IsLoggedIn())
                {
                    var task = CartService.GetCartAsync(SettingsService.AuthAccessToken);
                    await Task.WhenAll(task, Task.Delay(1000));
                    var result = await task;
                    foreach (var cart in result)
                    {
                        TotalAmount += cart.Amount;
                        Cart.Add(cart);
                    }
                }
                else
                {
                    string ids = string.Join(",", Barrel.Current.GetKeys());
                    var task = ProductService.GetBulkProductsAsync(ids);
                    await Task.WhenAll(task, Task.Delay(1000));
                    var result = await task;

                    foreach (var product in result)
                    {
                        int quantity = Barrel.Current.Get<int>(product.Id.ToString());
                        TotalAmount += (quantity * product.Price);
                        Cart.Add(new Cart
                        {
                            Id = product.Id,
                            Quantity = quantity,
                            Product = product
                        });
                    }
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
                    State = Cart.Count > 0 ? LayoutState.None : LayoutState.Empty;
                }
            }
        }
    }
}
