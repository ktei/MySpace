using Caliburn.Micro;
using System;
using System.Collections.Generic;

namespace LiteApp.MySpace.ViewModels
{
    public class AlbumViewModel : PropertyChangedBase
    {
        string _name;
        string _description;
        string[] _coverURIs = DefaultCoverURIs;
        bool _isLoadingCover = true;
        
        public static readonly string DefaultCoverURI = "../Assets/Images/picture.png";
        public static readonly string[] DefaultCoverURIs = new string[] { DefaultCoverURI, DefaultCoverURI, DefaultCoverURI };

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

        public string[] CoverURIs
        {
            get { return _coverURIs; }
            set
            {
                if (_coverURIs != value)
                {
                    _coverURIs = value;
                    NotifyOfPropertyChange(() => CoverURIs);
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

        public static string[] CovertToCoverURIs(string combinedCoverURIs)
        {
            string[] splits = combinedCoverURIs.Split(";".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
            if (splits.Length == 0)
                return DefaultCoverURIs;
            string[] coverURIs = new string[] { DefaultCoverURI, DefaultCoverURI, DefaultCoverURI };
            for (int i = 0; i < Math.Min(3, splits.Length); ++i)
            {
                coverURIs[i] = splits[i];
            }
            return coverURIs;
        }

        public static string[] CovertToCoverURIs(IList<string> uris)
        {
            if (uris == null)
                return DefaultCoverURIs;
            if (uris.Count == 0)
                return DefaultCoverURIs;
            string[] coverURIs = new string[] { DefaultCoverURI, DefaultCoverURI, DefaultCoverURI };
            for (int i = 0; i < Math.Min(3, uris.Count); ++i)
            {
                coverURIs[i] = uris[i];
            }
            return coverURIs;
        }
    }
}
