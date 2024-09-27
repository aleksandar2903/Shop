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
    public partial class PromotionPage : ContentPage
    {
        int id;
        PromotionViewModel ViewModel { get; }
        public PromotionPage(int promotionId)
        {
            InitializeComponent();
            BindingContext = ViewModel = new PromotionViewModel();
            this.id = promotionId;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ViewModel.OnInitialize(id);
        }
    }
}