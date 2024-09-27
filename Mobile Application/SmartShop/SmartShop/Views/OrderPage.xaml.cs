using SmartShop.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage
    {
        OrdersViewModel viewModel;
        public OrderPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new OrdersViewModel();
        }

        protected override async void OnAppearing()
        {
            await viewModel.InitializeAsync();
        }
    }
}