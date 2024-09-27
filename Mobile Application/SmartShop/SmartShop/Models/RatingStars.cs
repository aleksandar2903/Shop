using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace SmartShop.Models
{
    public class RatingStars
    {
        int _value;
        public int Value { get => _value; set { _value = value;  SetStars(); } }
        public ObservableCollection<Star> Stars { get; private set; } = new ObservableCollection<Star>();

        void SetStars()
        {
            Stars.Clear();

            if(Value >= 0)
            {
                for (int i = 0, n = 5; i < n; i++)
                {
                    var star = new Star { Value = i + 1, Text = "☆" };
                    if(i < Value)
                    {
                        star.Text = "★";
                        Stars.Add(star);
                    }
                    else
                    {
                        Stars.Add(star);
                    }
                }
            }
        }
    }
}
