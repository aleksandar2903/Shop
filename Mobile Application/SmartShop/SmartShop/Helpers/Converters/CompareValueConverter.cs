using MonkeyCache.FileStore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.CommunityToolkit.Extensions.Internals;
using Xamarin.Forms;

namespace SmartShop.Helpers.Converters
{
    public class CompareValueConverter : ValueConverterExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                return false;
            }

            if(value is int newValue)
            {
                if(newValue >= 0)
                {
                    if (Barrel.Current.Exists($"f-{newValue}"))
                    {
                        return true;
                    }
                }
                return false;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
