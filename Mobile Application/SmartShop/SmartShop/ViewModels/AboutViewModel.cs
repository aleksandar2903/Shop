using SmartShop.Views;
using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartShop.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenCategoriesPageCommand = new Command(async () => await Shell.Current.Navigation.PushAsync(new CategoriesPage(), true));
        }

        public Command OpenCategoriesPageCommand { get; }
    }
}