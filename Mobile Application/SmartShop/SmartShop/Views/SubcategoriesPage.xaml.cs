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
    public partial class SubcategoriesPage : ContentPage
    {
        SubcategoriesViewModel viewModel;
        string name;
        int categoryId;
        public SubcategoriesPage(Category category)
        {
            InitializeComponent();
            name = category.Name;
            categoryId = category.Id;
            BindingContext = viewModel = new SubcategoriesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnInitialize(name, categoryId);
        }
    }
}