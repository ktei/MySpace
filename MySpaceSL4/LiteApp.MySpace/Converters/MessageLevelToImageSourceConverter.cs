﻿using System;
using System.Windows.Data;
using LiteApp.MySpace.ViewModels;

namespace LiteApp.MySpace.Converters
{
    public class MessageLevelToImageSourceConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            MessageLevel level = (MessageLevel)value;
            if (level == MessageLevel.Information)
                return "../Assets/Images/information.png";
            else if (level == MessageLevel.Exclamation)
                return "../Assets/Images/exclamation.png";
            return "../Assets/Images/error.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}