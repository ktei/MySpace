using Caliburn.Micro;
using System;
using System.Collections.Generic;

namespace LiteApp.MySpace.ViewModels
{
    public class AlbumViewModel : Screen
    {
        string _name;
        string _description;
        CoverViewModel[] _covers;

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

        public CoverViewModel[] Covers
        {
            get { return _covers; }
            set
            {
                if (_covers != value)
                {
                    _covers = value;
                    NotifyOfPropertyChange(() => Covers);
                }
            }
        }

        public bool IsLoadingCover
        {
            get { return false; }
        }

        public static CoverViewModel[] GetCovers(string combinedCoverURIs)
        {
            string[] splits = combinedCoverURIs.Split(";".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
            CoverViewModel[] covers = new CoverViewModel[] { CoverViewModel.Default, CoverViewModel.Default, CoverViewModel.Default };
            for (int i = 0; i < Math.Min(3, splits.Length); ++i)
            {
                covers[i] = new CoverViewModel() { SourceURI = splits[i] };
            }
            return covers;
        }

        public static CoverViewModel[] GetCovers(IList<string> uris)
        {
            if (uris == null)
                return new CoverViewModel[] { CoverViewModel.Default, CoverViewModel.Default, CoverViewModel.Default };
            CoverViewModel[] covers = new CoverViewModel[] { CoverViewModel.Default, CoverViewModel.Default, CoverViewModel.Default };
            for (int i = 0; i < Math.Min(3, uris.Count); ++i)
            {
                covers[i] = new CoverViewModel() { SourceURI = uris[i] };
            }
            return covers;
        }
    }
}
