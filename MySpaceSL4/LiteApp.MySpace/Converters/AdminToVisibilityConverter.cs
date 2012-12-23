using System;
using System.Windows;
using System.Windows.Data;
using LiteApp.MySpace.Security;

namespace LiteApp.MySpace.Converters
{
    public class AdminToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return SecurityContext.Current.IsSuperAdminSignedIn() ? Visibility.Visible : Visibility.Collapsed;    
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
