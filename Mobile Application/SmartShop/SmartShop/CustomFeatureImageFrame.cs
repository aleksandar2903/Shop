using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartShop
{
    public class CustomFeatureImageFrame : Image
    {
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var info = DeviceDisplay.MainDisplayInfo;
            if(DeviceInfo.Idiom == DeviceIdiom.Phone)
                WidthRequest = info.Width / info.Density / 1.15;
        }
    }
}
