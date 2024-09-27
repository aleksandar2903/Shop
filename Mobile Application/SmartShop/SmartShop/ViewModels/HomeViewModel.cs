using SmartShop.Models;
using SmartShop.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace SmartShop.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private Item _selectedItem;
        public ObservableCollection<Item> Items { get; }
        public ObservableCollection<Category> Categories { get; }
        public ObservableCollection<Product> Products { get; }
        public ObservableCollection<Product> FeaturedProducts { get; }
        public ObservableCollection<Promotion> Promotions { get; }
        public ICommand LoadItemsCommand { get; }
        public ICommand OpenCategoriesPageCommand { get; }
        public ICommand AddItemCommand { get; }
        public ICommand ProductTapped { get; }
        public ICommand PromotionTapped { get; }
        public ICommand ToggleFavouriteProductCommand { get; }
        public ICommand CategoryTappedCommand { get; }


        public HomeViewModel()
        {
            Items = new ObservableCollection<Item>();
            Categories = new ObservableCollection<Category>();
            Products = new ObservableCollection<Product>();
            FeaturedProducts = new ObservableCollection<Product>();
            Promotions = new ObservableCollection<Promotion>();
            LoadItemsCommand = new Command(async () => await LoadDataAsync());
            OpenCategoriesPageCommand = new Command(async () => await Shell.Current.Navigation.PushAsync(new ExplorePage(), true));
            PromotionTapped = new Command<Promotion>(async (promotion) => await Shell.Current.Navigation.PushAsync(new PromotionPage(promotion.Id), true));
            ToggleFavouriteProductCommand = new Command<Product>(async (product) => await ToggleProduct(product));
            CategoryTappedCommand = new Command<Category>(async (category) => await Shell.Current.Navigation.PushAsync(new SubcategoriesPage(category)));
        }

        

        public override async Task InitializeAsync()
        {
            SelectedItem = null;
            if (Products.Count == 0)
            {
                await LoadDataAsync();
            }
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
            }
        }

        public int FrameSize { get; }
        private async Task LoadDataAsync()
        {
            if (!VerifyInternetConnection())
            {
                State = LayoutState.Custom;
                CustomStateKey = StateKeys.Offline;
                return;
            }

            State = LayoutState.Loading;
            try
            {
                var categoriesTask = CategoryBrandService.GetCategoriesAsync();
                var productsTask = ProductService.GetPopularProductsAsync(SettingsService.AuthAccessToken);
                var featuredProductsTask = ProductService.GetNewestProductsAsync(SettingsService.AuthAccessToken);
                var promotionsTask = PromotionService.GetPromotions();

                await Task.WhenAll(featuredProductsTask, categoriesTask, productsTask, featuredProductsTask, promotionsTask, Task.Delay(1000));

                var featuredProducts = await featuredProductsTask;
                var products = await productsTask;
                var categories = await categoriesTask;
                var promotions = await promotionsTask;


                Categories.Clear();
                foreach (var category in categories)
                {
                    Categories.Add(category);
                }

                Products.Clear();
                foreach (var product in products)
                {
                    Products.Add(product);
                }

                FeaturedProducts.Clear();
                foreach (var product in featuredProducts)
                {
                    FeaturedProducts.Add(product);
                }

                Promotions.Clear();
                foreach (var promotion in promotions)
                {
                    Promotions.Add(promotion);
                }

            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                State = LayoutState.Error;
            }
            finally
            {
                if (State != LayoutState.Error)
                {
                    State = Categories.Count > 0 && FeaturedProducts.Count > 0 && Products.Count > 0 && Promotions.Count > 0 ? LayoutState.None : LayoutState.Empty;
                }
            }
        }
        protected override async Task RefreshData()
        {
            await LoadDataAsync();
        }
        private async Task ToggleProduct(Product product)
        {
            if (!IsLoggedIn())
            {
                await Shell.Current.GoToAsync(nameof(LoginPage), true);
                return;
            }
            if (!VerifyInternetConnection())
            {
                return;
            }

            product.Favourite = !product.Favourite;

            try
            {
                await FavouriteService.ToogleFavourite(product.Id, SettingsService.AuthAccessToken);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                product.Favourite = !product.Favourite;
            }
        }
    }
}