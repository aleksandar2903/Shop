using SmartShop.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        RegisterViewModel viewModel;
        public RegisterPage()
        {
            InitializeComponent();
            this.BindingContext = viewModel = new RegisterViewModel();
        }

        protected override async void OnAppearing()
        {
            await viewModel.InitializeAsync();
        }
    }
}