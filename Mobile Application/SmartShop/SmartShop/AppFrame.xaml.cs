using SmartShop.Components.Navbar;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppFrame : Grid
    {
        public AppFrame()
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

        public static readonly BindableProperty UserLogoIsVisibleProperty =
        BindableProperty.Create(
            nameof(UserLogoIsVisible),
            typeof(bool),
            typeof(Navbar),
            true);

        public bool UserLogoIsVisible
        {
            get { return (bool)GetValue(UserLogoIsVisibleProperty); }
            set { SetValue(UserLogoIsVisibleProperty, value); }
        }

        public static readonly BindableProperty SearchLogoIsVisibleProperty =
        BindableProperty.Create(
            nameof(SearchLogoIsVisible),
            typeof(bool),
            typeof(Navbar),
            true);

        public bool SearchLogoIsVisible
        {
            get { return (bool)GetValue(SearchLogoIsVisibleProperty); }
            set { SetValue(SearchLogoIsVisibleProperty, value); }
        }

        public static readonly BindableProperty ContentCenterProperty =
        BindableProperty.Create(
            nameof(ContentCenter),
            typeof(ContentView),
            typeof(Navbar));

        public ContentView ContentCenter
        {
            get { return (ContentView)GetValue(ContentCenterProperty); }
            set { SetValue(ContentCenterProperty, value); }
        }

        public static readonly BindableProperty ContentEndProperty =
        BindableProperty.Create(
            nameof(ContentEnd),
            typeof(ContentView),
            typeof(Navbar));

        public ContentView ContentEnd
        {
            get { return (ContentView)GetValue(ContentEndProperty); }
            set { SetValue(ContentEndProperty, value); }
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
        public static readonly BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(AppFrame));
        public View Content
        {
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        public static readonly BindableProperty EmptyTemplateProperty =
            BindableProperty.Create(nameof(EmptyTemplate), typeof(DataTemplate), typeof(AppFrame), Application.Current.Resources["empty"] as DataTemplate);
        public DataTemplate EmptyTemplate
        {
            get => (DataTemplate)GetValue(EmptyTemplateProperty);
            set => SetValue(EmptyTemplateProperty, value);
        }
        public static readonly BindableProperty LoadingTemplateProperty =
            BindableProperty.Create(nameof(LoadingTemplate), typeof(DataTemplate), typeof(AppFrame), Application.Current.Resources["loading"] as DataTemplate);
        public DataTemplate LoadingTemplate
        {
            get => (DataTemplate)GetValue(LoadingTemplateProperty);
            set => SetValue(LoadingTemplateProperty, value);
        }
    }
}