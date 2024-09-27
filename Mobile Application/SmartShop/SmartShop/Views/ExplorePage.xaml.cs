using SmartShop.Models;
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
    public partial class ExplorePage : ContentPage
    {
        BrowseViewModel viewModel;
        string subcategories;
        string brand;
        public ExplorePage(string subcategories = "", string brand = "")
        {
            InitializeComponent();
          
            BindingContext = viewModel = new BrowseViewModel();
            this.subcategories = subcategories;
            this.brand = brand;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!viewModel.IsInitialized && string.IsNullOrWhiteSpace(brand) && string.IsNullOrWhiteSpace(subcategories))
            {
                await Task.Delay(600);
                Entry entry = this.FindByName<Entry>("searchEntry");
                entry.Focus();
            }
            viewModel.OnAppearing(subcategories, brand);
        }
    }
}