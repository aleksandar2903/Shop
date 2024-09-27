using SmartShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.Components.Frames
{
    public partial class ProductFrame : StackLayout
    {
        public ProductFrame()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ToggleFavouriteProductCommandProperty =
        BindableProperty.Create(
            nameof(ToggleFavouriteProductCommand),
            typeof(ICommand),
            typeof(ProductFrame),
            default(string));

        public ICommand ToggleFavouriteProductCommand
        {
            get { return (ICommand)GetValue(ToggleFavouriteProductCommandProperty); }
            set { SetValue(ToggleFavouriteProductCommandProperty, value); }
        }
    }
}