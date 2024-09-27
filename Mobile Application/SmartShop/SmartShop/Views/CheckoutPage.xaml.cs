using SmartShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckoutPage : ContentPage
    {
        CheckoutViewModel ViewModel;
        public CheckoutPage()
        {
            InitializeComponent();
            BindingContext = ViewModel = DependencyService.Get<CheckoutViewModel>();
        }

        protected override async void OnAppearing()
        {
            await ViewModel.InitializeAsync();
        }
    }
}