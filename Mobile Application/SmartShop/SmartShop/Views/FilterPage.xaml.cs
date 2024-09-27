using SmartShop.Models;
using SmartShop.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.Views
{
    public partial class FilterPage : ContentPage
    {
        public string Query { get; set; }
        public string Categories { get; set; }
        public string Brand { get; set; }
        FilterViewModel ViewModel { get; }
        public FilterPage(FilterViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = ViewModel = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnInitialize(Query, Categories, Brand);
        }
    }
}