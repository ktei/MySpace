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
                case PhotoUploadStatus.Uploading:
                case PhotoUploadStatus.Completing:
                    return "../Assets/Images/upload-16.png";
                case PhotoUploadStatus.Completed:
                    return "../Assets/Images/success-16.png";
                case PhotoUploadStatus.Canceled:
                case PhotoUploadStatus.Error:
                    return "../Assets/Images/canceled-16.png";
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
