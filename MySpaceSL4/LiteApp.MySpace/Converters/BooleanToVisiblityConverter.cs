﻿using System;
using System.Windows;
using System.Windows.Data;

namespace LiteApp.MySpace.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public bool Reverse { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if (Reverse)
                    return (bool)value ? Visibility.Collapsed : Visibility.Visible;
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
