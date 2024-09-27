using SmartShop.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;

namespace SmartShop.ViewModels
{
    internal class OrdersViewModel : BaseViewModel
    {
        public ObservableCollection<Order> Orders { get;  }
        public OrdersViewModel()
        {
            Orders = new ObservableCollection<Order>();
        }
        public override async Task InitializeAsync()
        {

            await LoadDataAsync();
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
                var ordersTask = OrderService.GetOrdersAsync(SettingsService.AuthAccessToken);

                await Task.WhenAll(ordersTask, Task.Delay(1000));

                var orders = await ordersTask;


                Orders.Clear();
                foreach (var order in orders)
                {
                    Orders.Add(order);
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
                    State = Orders.Count > 0 ? LayoutState.None : LayoutState.Empty;
                }
            }
        }
        protected override async Task RefreshData()
        {
            await LoadDataAsync();
        }
    }
}
