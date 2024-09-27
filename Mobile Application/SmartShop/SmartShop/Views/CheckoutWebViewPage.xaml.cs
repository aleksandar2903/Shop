using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace SmartShop.Views
{	
	public partial class CheckoutWebViewPage : ContentPage
	{
		bool openedPage = true;

		public event EventHandler<string> CloseCheckoutPage;

		public CheckoutWebViewPage (string url)
		{
			InitializeComponent();
			checkoutWebView.Source = url;
		}

        void checkoutWebView_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
			var webView = sender as WebView;

			if (webView.Source != null)
			{
				var source = webView.Source as UrlWebViewSource;


				if (openedPage && (source.Url.Contains("/cancel") || source.Url.Contains("/success")))
				{
					openedPage = false;
					CloseCheckoutPage?.Invoke(this, source.Url.Contains("/cancel") ? "cancel" : "success");
                }
			}
        }
    }
}

