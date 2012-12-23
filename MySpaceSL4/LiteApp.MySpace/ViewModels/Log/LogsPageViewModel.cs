using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using LiteApp.MySpace.Framework;

namespace LiteApp.MySpace.ViewModels
{
    [Export(typeof(IPage))]
    [PageMetadata("Logs")]
    public class LogsPageViewModel : Screen, IPage
    {
        ServerSidePagedCollectionView<LogEntryViewModel> _entries;
        bool _isBusy;

        public string Name
        {
            get { return "Logs"; }
        }

        public bool HasLog
        {
            get { return _entries != null && _entries.Count > 0; }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    NotifyOfPropertyChange(() => IsBusy);
                }
            }
        }

        public IEnumerable<LogEntryViewModel> Entries
        {
            get { return _entries; }
            
        }

        public void RefreshData()
        {
            if (_entries != null)
            {
                _entries.RefreshCurrentPage();
            }
        }

        protected override void OnInitialize()
        {
            LoadEntries();
            base.OnInitialize();
        }

        void LoadEntries()
        {
            _entries = new ServerSidePagedCollectionView<LogEntryViewModel>(new LogsPagedDataSource()) { PageSize = 20 };
            _entries.RefreshDataFailed += _entries_RefreshDataFailed;
            _entries.PageChanging += _entries_PageChanging;
            _entries.PageChanged += _entries_PageChanged;
            _entries.MoveToFirstPage();
        }

        void _entries_RefreshDataFailed(object sender, RefreshPagedDataFailedEventArgs e)
        {
            e.Error.Handle();
        }

        void _entries_PageChanging(object sender, PageChangingEventArgs e)
        {
            IsBusy = true;
        }

        void _entries_PageChanged(object sender, EventArgs e)
        {
            IsBusy = false;
            NotifyOfPropertyChange(() => HasLog);
        }
    }
}
