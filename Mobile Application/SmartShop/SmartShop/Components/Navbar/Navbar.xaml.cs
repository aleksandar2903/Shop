using SmartShop.Components.Frames;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.Components.Navbar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Navbar : Grid
    {
        public Navbar()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty LogoIsVisibleProperty =
        BindableProperty.Create(
            nameof(LogoIsVisible),
            typeof(bool),
            typeof(Navbar),
            default(bool));

        public bool LogoIsVisible
        {
            get { return (bool)GetValue(LogoIsVisibleProperty); }
            set { SetValue(LogoIsVisibleProperty, value); }
        }

        public static readonly BindableProperty BackwardButtonIsVisibleProperty =
        BindableProperty.Create(
            nameof(BackwardButtonIsVisible),
            typeof(bool),
            typeof(Navbar),
            default(bool));

        public bool BackwardButtonIsVisible
        {
            get { return (bool)GetValue(BackwardButtonIsVisibleProperty); }
            set { SetValue(BackwardButtonIsVisibleProperty, value); }
        }

        public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(Navbar),
            default(string));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
    }
}