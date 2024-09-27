using SmartShop;
using SmartShop.IOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomSearchBar), typeof(CustomSearchBarRenderer))]
namespace SmartShop.IOS
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var searchbar = (UISearchBar)Control;
                searchbar.TintColor = UIColor.Orange;
                searchbar.BackgroundColor = UIColor.Clear;
            }
        }
    }
}