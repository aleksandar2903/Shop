using SmartShop.Models;
using SmartShop.Models.Request;
using SmartShop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartShop.ViewModels
{
    public enum PaymentTypes
    {
        Cart = 1,
        Cash = 2,
        Bank = 3
    };

    public class CheckoutViewModel : BaseViewModel
    {
        private string _firstName = "Aleksandar";
        private string _lastName = "Stojanovic";
        private string _phone = "+381644069038";
        private string _address = "Straza, bb";
        private string _city = "Gnjilane";
        private string _zipCode = "38252";
        private string _cardName = "Aleksandar Stojanovic";
        private string _cardNumber = "4242424242424242";
        private string _cvv = "123";
        private string _expireDate = "05/23";
        private PaymentTypes _paymentType = PaymentTypes.Cart;
        private decimal _totalAmount;
        private bool _cartChecked = true;
        private bool _cashChecked = false;
        private bool _bankChecked = false;

        public string FirstName { get => _firstName; set => SetProperty(ref _firstName, value); }
        public string LastName { get => _lastName; set => SetProperty(ref _lastName, value); }
        public string Phone { get => _phone; set => SetProperty(ref _phone, value); }
        public string Address { get => _address; set => SetProperty(ref _address, value); }
        public string City { get => _city; set => SetProperty(ref _city, value); }
        public string ZipCode { get => _zipCode; set => SetProperty(ref _zipCode, value); }
        public string CardName { get => _cardName; set => SetProperty(ref _cardName, value); }
        public string CardNumber { get => _cardNumber; set => SetProperty(ref _cardNumber, value); }
        public string Cvv { get => _cvv; set => SetProperty(ref _cvv, value); }
        public string ExpireDate { get => _expireDate; set => SetProperty(ref _expireDate, value); }
        public decimal TotalAmount { get => _totalAmount; set => SetProperty(ref _totalAmount, value); }
        public PaymentTypes PaymentType { get => _paymentType; set => SetProperty(ref _paymentType, value); }
        public bool CartChecked { get => _cartChecked;
            set
            {
                PaymentType = PaymentTypes.Cart;
                SetProperty(ref _cartChecked, value);
            }
        }
        public bool CashChecked
        {
            get => _cashChecked;
            set
            {
                PaymentType = PaymentTypes.Cash;
                SetProperty(ref _cashChecked, value);
            }
        }
        public bool BankChecked
        {
            get => _bankChecked;
            set
            {
                PaymentType = PaymentTypes.Bank;
                SetProperty(ref _bankChecked, value);
            }
        }
        public ObservableCollection<Cart> Cart { get; }
        public Command SaveShippingInformationsCommand { get; }
        public Command SavePaymentInformationsCommand { get; }
        public Command PlaceOrderCommand { get; }

        public CheckoutViewModel()
        {
            SaveShippingInformationsCommand = new Command(async () => await Shell.Current.Navigation.PushAsync(new PaymentPage(), true), () => ValidateShippingAddress());
            SavePaymentInformationsCommand = new Command(async () => await Shell.Current.Navigation.PushAsync(new CheckoutPage(), true));
            PlaceOrderCommand = new Command(async () => await AddOrderAsync(), () => ValidateCardInformations());
            Cart = new ObservableCollection<Cart>();
            this.PropertyChanged +=
                (_, __) => SaveShippingInformationsCommand.ChangeCanExecute();
            this.PropertyChanged +=
                (_, __) => PlaceOrderCommand.ChangeCanExecute();
        }


        public override async Task InitializeAsync()
        {
            await LoadDataAsync();
        }
        private async Task AddOrderAsync()
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
                var order = new OrderRequest(TotalAmount,
                    new ShippingAddress(FirstName + " " + LastName, "", Phone, Address, City, ZipCode),
                    (int)PaymentType, Cart.Select(s => new Cart(s.Quantity, s.ProductId, s.Price, s.Amount)).ToList());
                var orderTask = OrderService.AddOrderAsync(order, SettingsService.AuthAccessToken);

                await Task.WhenAll(orderTask, Task.Delay(1000));

                var checkoutUrl = await orderTask;

                if (PaymentType == PaymentTypes.Cart)
                {
                    var checkoutPageModal = new CheckoutWebViewPage(checkoutUrl);

                    checkoutPageModal.CloseCheckoutPage += CloseCheckoutPageModal;

                    await Shell.Current.Navigation.PushModalAsync(checkoutPageModal);
                } else
                {
                    await Task.Delay(3000).ContinueWith(t => Shell.Current.Navigation.PopToRootAsync());
                }

            }

            catch (Exception ex)
            {
                State = LayoutState.Error;
            }
            finally
            {
                State = LayoutState.None;
            }
        }

        private void InitProperties()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Phone = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            ZipCode = string.Empty;
            CardNumber = string.Empty;
            CardName = string.Empty;
            Cvv = string.Empty;
            ExpireDate = string.Empty;
            TotalAmount = decimal.Zero;
            Cart.Clear();
        }
        private bool ValidateShippingAddress()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                !string.IsNullOrWhiteSpace(LastName) &&
                !string.IsNullOrWhiteSpace(Address) &&
                !string.IsNullOrWhiteSpace(City) &&
                Phone.Length == 13 && ZipCode.Length == 5;
        }
        private bool ValidateCardInformations()
        {
            return !string.IsNullOrWhiteSpace(CardName) &&
                ExpireDate.Length == 5 &&
                CardNumber.Length == 16 &&
                Cvv.Length == 3;
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
                var cartTask = CartService.GetCartAsync(SettingsService.AuthAccessToken);

                await Task.WhenAll(cartTask, Task.Delay(1000));

                var cart = await cartTask;

                TotalAmount = decimal.Zero;
                Cart.Clear();
                foreach (var product in cart)
                {
                    TotalAmount += product.Amount;
                    Cart.Add(product);
                }
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
                    State = Cart.Count > 0 ? LayoutState.None : LayoutState.Error;
                }
            }
        }
        private async void CloseCheckoutPageModal(object sender, string e)
        {
            await Task.Delay(3000).ContinueWith(t => Shell.Current.Navigation.PopToRootAsync());

            ((CheckoutWebViewPage)sender).CloseCheckoutPage -= CloseCheckoutPageModal;
        }
    }
}
