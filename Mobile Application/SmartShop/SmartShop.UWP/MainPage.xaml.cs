namespace SmartShop.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new SmartShop.App());
        }
    }
}
