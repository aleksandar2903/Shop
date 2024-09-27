using SmartShop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartShop.ViewModels
{
    public class ReviewsViewModel : BaseViewModel
    {
        public RatingStars RatingStars { get; set; } = new RatingStars();

        public ObservableCollection<Review> Reviews { get; set; }

        public ICommand SelectRatingCommand { get; }

        public ReviewsViewModel()
        {
            Reviews = new ObservableCollection<Review>();
            RatingStars.Value = 0;
            SelectRatingCommand = new Command<Star>((star) => RatingStars.Value = star.Value);
        }
    }
}
