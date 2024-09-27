using SmartShop.Models;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.Components.Frames
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SimilarProductFrame : StackLayout
    {
        public Product Product { get; set; }
        public SimilarProductFrame()
        {
            InitializeComponent();
            this.BindingContext = this;
        }
    }
}