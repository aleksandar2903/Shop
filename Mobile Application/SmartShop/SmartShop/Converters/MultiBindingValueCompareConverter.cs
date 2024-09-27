using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace SmartShop.Converters
{
    internal class MultiBindingValueCompareConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values == null || values.Length < 2)
                    return false;

                if (values[0] == null || values[1] == null)
                    return false;

                if (values[0] == values[1])
                    return true;
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
