using SmartShop.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShippingPage : ContentPage
    {
        public ShippingPage()
        {
            InitializeComponent();
            BindingContext = DependencyService.Get<CheckoutViewModel>();
        }
    }
}