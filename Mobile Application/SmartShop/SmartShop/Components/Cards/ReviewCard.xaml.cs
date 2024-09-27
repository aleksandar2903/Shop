using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.Components.Cards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReviewCard : StackLayout
    {
        public ReviewCard()
        {
            InitializeComponent();
        }
    }
}