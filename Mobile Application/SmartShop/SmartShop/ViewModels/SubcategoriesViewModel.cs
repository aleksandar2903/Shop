using SmartShop.Models;
using SmartShop.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace SmartShop.ViewModels
{
    public class SubcategoriesViewModel : BaseViewModel
    {
        public ObservableCollection<Subcategory> Subcategories { get; }
        public Command SubcategoryTapped { get; }
        int categoryId;
        public SubcategoriesViewModel()
        {
            Subcategories = new ObservableCollection<Subcategory>();
            SubcategoryTapped = new Command<Subcategory>(OnSelectedSubcategory);
        }

        public async void OnInitialize(string name, int categoryId)
        {
            if(categoryId > 0 && Subcategories.Count == 0)
            {
                Title = name;
                this.categoryId = categoryId;
                await LoadDataAsync(categoryId);
            }
        } 

        async Task LoadDataAsync(int categoryId)
        {
            if (!VerifyInternetConnection())
            {
                State = LayoutState.Custom;
                CustomStateKey = StateKeys.Offline;
                return;
            }

            State = LayoutState.Loading;

            await Task.Delay(600);

            try
            {
                Subcategories.Add(new Subcategory { Name = "Vidi sve"});
                var subcategories = await CategoryBrandService.GetSubcategoriesAsync(categoryId);

                foreach (var subcategory in subcategories)
                {
                    Subcategories.Add(subcategory);
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
                    State = Subcategories.Count > 0 ? LayoutState.None : LayoutState.Empty;
                }
            }
        }
        protected override async Task RefreshData()
        {
            await LoadDataAsync(categoryId);
        }
        async void OnSelectedSubcategory(Subcategory subcategory)
        {
            if (subcategory == null)
                return;

            string subcategories = "";

            if (subcategory.Id == 0)
                subcategories = String.Join(",", Subcategories.Select(s => s.Id));
            else
                subcategories = subcategory.Id.ToString();

            await Shell.Current.Navigation.PushAsync(new ExplorePage(subcategories));
        }
    }
}
