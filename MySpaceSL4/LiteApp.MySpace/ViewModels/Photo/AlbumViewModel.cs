using Caliburn.Micro;

namespace LiteApp.MySpace.ViewModels
{
    public class AlbumViewModel : PropertyChangedBase
    {
        string _name;
        string _description;
        string _coverUri;
        bool _isLoadingCover = true;

        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyOfPropertyChange(() => Name);
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    NotifyOfPropertyChange(() => Description);
                }
            }
        }

        public string CoverUri
        {
            get { return _coverUri; }
            set
            {
                if (_coverUri != value)
                {
                    _coverUri = value;
                    NotifyOfPropertyChange(() => CoverUri);
                }
            }
        }

        public bool IsLoadingCover
        {
            get { return _isLoadingCover; }
            set
            {
                if (_isLoadingCover != value)
                {
                    _isLoadingCover = value;
                    NotifyOfPropertyChange(() => IsLoadingCover);
                }
            }
        }
    }
}
