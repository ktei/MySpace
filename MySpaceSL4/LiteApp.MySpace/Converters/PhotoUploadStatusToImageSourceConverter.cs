using System;
using System.Windows;
using System.Windows.Data;
using LiteApp.MySpace.ViewModels;

namespace LiteApp.MySpace.Converters
{
    public class PhotoUploadStatusToImageSourceConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            PhotoUploadStatus status = (PhotoUploadStatus)value;
            switch (status)
            {
                case PhotoUploadStatus.Completing:
                    return "../Assets/Images/completing-16.png";
                case PhotoUploadStatus.Succeeded:
                    return "../Assets/Images/success-16.png";
                case PhotoUploadStatus.Canceled:
                    return "../Assets/Images/cancel-16.png";
                case PhotoUploadStatus.Error:
                    return "../Assets/Images/error-16.png";
                default:
                    return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
