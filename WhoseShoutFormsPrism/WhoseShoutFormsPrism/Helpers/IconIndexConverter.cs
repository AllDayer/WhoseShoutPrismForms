﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WhoseShoutFormsPrism.Helpers
{
    public class IconIndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if((int)value == -1)
            {
                return CurrentApp.Current.MainViewModel.Icons[0];
            }
            return CurrentApp.Current.MainViewModel.Icons[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}
