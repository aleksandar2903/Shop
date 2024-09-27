using SmartShop.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.Views
{
    [QueryProperty("disableLoad", "disabledLoad")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        ProfileViewModel viewModel;
        public bool? disableLoad { set; get; } = false;
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ProfileViewModel();
        }

        protected override async void OnAppearing()
        {
            if (!(bool)disableLoad)
            {
                await viewModel.InitializeAsync();
            }
        }
    }
}