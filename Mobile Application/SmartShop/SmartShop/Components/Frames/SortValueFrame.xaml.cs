using SmartShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.Components.Frames
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SortValueFrame : StackLayout
    {
        public SortValueFrame()
        {
            InitializeComponent();
        }

        //protected override void OnBindingContextChanged()
        //{
        //    base.OnBindingContextChanged();

        //    if (this.BindingContext != null)
        //    {
        //        bool isSelected = false;

        //        if (this.BindingContext is Brand brand)
        //        {
        //            if (brand.IsActive)
        //                isSelected = true;
        //        }
        //        else if (this.BindingContext is Subcategory sub)
        //        {
        //            if (sub.IsActive)
        //                isSelected = true;
        //        }

        //        if (isSelected)
        //        {
        //            checkBox.Source = ImageSource.FromFile("checkbox.svg");
        //            name.FontAttributes = FontAttributes.Bold;
        //        }
        //    }
        //}
    }
}