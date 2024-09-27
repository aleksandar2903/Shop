using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.Extensions;
using MonkeyCache.FileStore;
using Xamarin.Essentials;
using SmartShop.Models;

namespace SmartShop.Components.Buttons
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouriteButton : ImageButton
    {
        //public Product Product { get; set; }
        public FavouriteButton()
        {
            InitializeComponent();
        }
        //private async void ImageButton_Clicked(object sender, EventArgs e)
        //{
        //    var imageButton = sender as ImageButton;

        //    if (this.BindingContext == null) return;

        //    if (this.BindingContext is Product product && product.Id >= 0)
        //    {
        //        if (Barrel.Current.Exists($"f-{product.Id}"))
        //        {
        //            imageButton.Source = "heart.png";
        //            Barrel.Current.Empty($"f-{product.Id}");
        //            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
        //            {
        //                await Shell.Current.DisplayToastAsync("Removed from favourites", 3000);
        //            }
        //        }
        //        else
        //        {
        //            imageButton.Source = "red_heart.png";
        //            Barrel.Current.Add($"f-{product.Id}", product.Id, TimeSpan.FromDays(90));
        //            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
        //            {
        //                await Shell.Current.DisplayToastAsync("Added to favourites", 3000);
        //            }
        //        }

        //    }
        //}

        //protected override void OnBindingContextChanged()
        //{
        //    base.OnBindingContextChanged();
        //    if (this.BindingContext == null) return;

        //    if (this.BindingContext is Product product && product.Id >= 0)
        //    {
        //        if (Barrel.Current.Exists($"f-{product.Id}"))
        //        {
        //            this.Source = "red_heart.png";
        //        }
        //        else
        //        {
        //            this.Source = "heart.png";
        //        }

        //    }
        //}
    }
}