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
    public partial class FavouritesPage : ContentPage
    {
        FavouritesViewModel viewModel; 
        public FavouritesPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new FavouritesViewModel();
        }

        protected async override void OnAppearing()
        {
            await viewModel.InitializeAsync();
        }

        protected override void OnDisappearing()
        {
            
        }
    }
}