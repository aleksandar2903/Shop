using SmartShop.ViewModels;
using Xamarin.Forms;

namespace SmartShop.Views
{
    public partial class HomePage : ContentPage
    {
        public HomeViewModel viewModel;
        public HomePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new HomeViewModel();
        }

        protected async override void OnAppearing()
        {
            await viewModel.InitializeAsync();
        }
    }
}