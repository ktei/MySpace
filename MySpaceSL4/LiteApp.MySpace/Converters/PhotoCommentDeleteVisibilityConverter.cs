﻿using System;
using System.Windows;
using System.Windows.Data;
using LiteApp.MySpace.Security;

namespace LiteApp.MySpace.Converters
{
    public class PhotoCommentDeleteVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string createdBy = System.Convert.ToString(value);
            return (SecurityContext.Current.IsSuperAdminSignedIn() || SecurityContext.Current.IsUserSignedIn(createdBy)) ? 
                Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
