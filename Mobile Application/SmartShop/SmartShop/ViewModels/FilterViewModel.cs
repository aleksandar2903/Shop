using SmartShop.Models;
using SmartShop.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace SmartShop.ViewModels
{
    public class FilterViewModel : BaseViewModel
    {
        private decimal minPrice;
        private decimal maxPrice;
        private decimal minPriceValidation;
        private decimal maxPriceValidation;
        private int totalRecords;
        private string query = "initialize";
        private string categories = "";
        private string brand = "";
        private SortEnum sortBy = SortEnum.Name;
        public event EventHandler<FilterRequest> FilterChanged;
        public ICommand OnCategorySelectCommand { get; }
        public ICommand OnPriceChangedCommand { get; }
        public ICommand OnBrandSelectCommand { get; }
        public ICommand OnResetTappedCommand { get; }
        public ICommand ApplyFiltersCommand { get; }
        public ObservableCollection<Brand> Brands { get; set; }
        public ObservableCollection<Subcategory> Categories { get; set; }
        public int TotalRecords { get => totalRecords; set { SetProperty(ref totalRecords, value); } }
        public SortEnum SortBy { get => sortBy; set { sortBy = value; } }
        public List<Sort> SortList
        {
            get => new List<Sort>
            {
                new Sort
                {
                    Value = SortEnum.Name,
                    Name = "Naziv A - Z"
                },
                new Sort
                {
                    Value = SortEnum.NameDESC,
                    Name = "Naziv Z - A"
                },
                new Sort
                {
                    Value = SortEnum.Price,
                    Name = "Jeftinije"
                },
                new Sort
                {
                    Value = SortEnum.PriceDESC,
                    Name = "Skuplje"
                }
            };
        }
        public decimal MinPrice
        {
            get => minPrice;
            set => SetProperty(ref minPrice, value >= 0 ? value : 0);
        }
        public decimal MaxPrice
        {
            get => maxPrice;
            set => SetProperty(ref maxPrice, value <= 9999999 ? value : 9999999);
        }
        public Dictionary<int, int> SelectedCategories { get; set; }
        public Dictionary<int, int> SelectedBrands { get; set; }

        public FilterViewModel()
        {
            Brands = new ObservableCollection<Brand>();
            Categories = new ObservableCollection<Subcategory>();
            SelectedBrands = new Dictionary<int, int>();
            SelectedCategories = new Dictionary<int, int>();
            OnBrandSelectCommand = new Command<int>(OnBrandSelect);
            OnCategorySelectCommand = new Command<int>(OnCategorySelect);
            OnResetTappedCommand = new Command(async () =>
            {
                ResetFilters();
                await FilterProducts();
            });
            OnPriceChangedCommand = new Command(async () =>
            {
                if (MinPrice <= MaxPrice)
                    await FilterProducts();
            });
            ApplyFiltersCommand = new Command(OnAppliedFilters);
        }

        void ResetFilters()
        {
            SelectedBrands.Clear();
            SelectedCategories.Clear();
            SetPrice();
        }

        void SetPrice(decimal? minPrice = 0, decimal? maxPrice = 0)
        {
            MaxPrice = maxPriceValidation = maxPrice != null ? (decimal)maxPrice : 0;
            MinPrice = minPriceValidation = minPrice != null ? (decimal)minPrice : 0;
        }

        public async void OnInitialize(string query = "", string categories = "", string brand = "")
        {
            this.categories = categories;
            this.brand = brand;
            if (this.query != query)
            {
                this.query = query;
                SetPrice();
                IsBusy = true;
                State = LayoutState.Loading;
                await Task.Delay(600);
                await FilterProducts();
            }
        }

        async void OnCategorySelect(int categoryId)
        {
            if (SelectedCategories.ContainsKey(categoryId))
                SelectedCategories.Remove(categoryId);
            else
                SelectedCategories.Add(categoryId, categoryId);

            SetPrice();

            await FilterProducts();
        }

        async void OnAppliedFilters()
        {
            var request = new FilterRequest
            {
                Brands = String.Join(",", SelectedBrands.Keys),
                Categories = String.Join(",", SelectedCategories.Keys),
                MinPrice = MinPrice,
                MaxPrice = MaxPrice,
                SortBy = SortBy.ToString()
            };
            FilterChanged.Invoke(this, request);

            await Shell.Current.Navigation.PopModalAsync();
        }

        async void OnBrandSelect(int brandId)
        {
            if (SelectedBrands.ContainsKey(brandId))
                SelectedBrands.Remove(brandId);
            else
                SelectedBrands.Add(brandId, brandId);

            SetPrice();

            await FilterProducts();
        }

        async Task FilterProducts()
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
                string categories = String.Join(",", SelectedCategories.Keys);
                string brands = String.Join(",", SelectedBrands.Keys);

                if (String.IsNullOrWhiteSpace(categories) && !String.IsNullOrWhiteSpace(this.categories))
                    categories = this.categories;

                if (String.IsNullOrWhiteSpace(brands) && !String.IsNullOrWhiteSpace(brand))
                    brands = brand;

                var responseTask = SearchService.FilterProducts(query, categories, brands, MinPrice, MaxPrice);

                await Task.WhenAll(responseTask, Task.Delay(1000));

                var response = await responseTask;

                if (response != null)
                {
                    TotalRecords = response.TotalRecords;
                    SetPrice(response.MinProductPrice, response.MaxProductPrice);
                    if (response.Categories != null && response.Categories.Count > 0 && String.IsNullOrWhiteSpace(this.categories))
                    {
                        Categories.Clear();
                        foreach (var category in response.Categories)
                        {
                            Categories.Add(category);
                        }
                    }
                    if (response.Brands != null && response.Brands.Count > 0 && String.IsNullOrWhiteSpace(brand))
                    {
                        Brands.Clear();
                        foreach (var brand in response.Brands)
                        {
                            Brands.Add(brand);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                State = LayoutState.Error;
            }
            finally
            {
                if (State != LayoutState.Error)
                {
                    State = Categories.Count > 0 || Brands.Count > 0 ? LayoutState.None : LayoutState.Empty;
                }
            }
        }

        protected override async Task RefreshData()
        {
            await FilterProducts();
        }
    }
}
