using Caliburn.Micro;

namespace LiteApp.MySpace.ViewModels
{
    public class CoverViewModel : PropertyChangedBase
    {
        bool _isLoading = true;
        string _sourceURI;
        static readonly string _defaultSourceURI = "../../Assets/Images/photo.png";

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

        public string SourceURI
        {
            get { return _sourceURI; }
            set
            {
                if (_sourceURI != value)
                {
                    _sourceURI = value;
                    NotifyOfPropertyChange(() => SourceURI);
                }
            }
        }

        public string DefaultSourceURI
        {
            get { return _defaultSourceURI; }
        }

        public static CoverViewModel Default
        {
            get
            {
                return new CoverViewModel() { _isLoading = false, _sourceURI = _defaultSourceURI };
            }
        }
    }
}
