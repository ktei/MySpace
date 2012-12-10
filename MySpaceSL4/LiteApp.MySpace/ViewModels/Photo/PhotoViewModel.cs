using System;
using Caliburn.Micro;

namespace LiteApp.MySpace.ViewModels
{
    public class PhotoViewModel : Screen
    {
        static readonly string _defaultThumbURI = "../Assets/Images/photo.png";

        bool _isLoading = true;

        public string Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Description { get; set; }

        public string PhotoURI { get; set; }

        public string ThumbURI { get; set; }


        /// <summary>
        /// Gets a value indicating whether the image is being loaded
        /// </summary>
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    NotifyOfPropertyChange(() => IsLoading);
                }
            }
        }

        public static string DefaultThumbURI
        {
            get { return _defaultThumbURI; }
        }
    }
}
