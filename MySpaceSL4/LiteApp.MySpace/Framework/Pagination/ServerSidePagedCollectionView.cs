using System;
using System.ComponentModel;
using Caliburn.Micro;
using System.Collections.Generic;

namespace LiteApp.MySpace.Framework
{
    public class ServerSidePagedCollectionView<T> : BindableCollection<T>, IPagedCollectionView
    {
        IPagedDataSource<T> _pagedDataSource;
        int _pageIndex;
        int _pageSize = 5;
        int _itemCount;
        int _totalItemCount;
        bool _isPageChanging;
        bool _canChangePage;

        public ServerSidePagedCollectionView(IPagedDataSource<T> pagedDataSource)
        {
            _pagedDataSource = pagedDataSource;
        }

        public event EventHandler<RefreshPagedDataFailedEventArgs> RefreshDataFailed;

        public bool MoveToFirstPage()
        {
            RefreshData(0);
            return true;
        }

        public bool MoveToLastPage()
        {
            RefreshData(TotalPages - 1);
            return true;
        }

        public bool MoveToNextPage()
        {
            RefreshData(PageIndex + 1);
            return true;
        }

        public bool MoveToPage(int pageIndex)
        {
            RefreshData(pageIndex);
            return true;
        }

        public bool MoveToPreviousPage()
        {
            RefreshData(PageIndex - 1);
            return true;
        }

        public bool RefreshCurrentPage()
        {
            RefreshData(_pageIndex);
            return true;
        }

        /// <summary>
        /// Fetches the data for the given page
        /// </summary>
        private void RefreshData(int newPageIndex)
        {
            try
            {
                // set the pre-fetch state
                Execute.OnUIThread(() =>
                    {
                        CanChangePage = false;
                        IsPageChanging = true;
                    });
                if (PageChanging != null)
                    PageChanging(this, new PageChangingEventArgs(newPageIndex));
                
                _pagedDataSource.FetchData(newPageIndex, PageSize, response =>
                {
                    // process the received data
                    this.DataReceived(response);

                    Execute.OnUIThread(() =>
                        {
                            PageIndex = newPageIndex;
                            IsPageChanging = false;
                            CanChangePage = true;
                        });


                    // set the post-fetch state
                    if (PageChanged != null)
                        PageChanged(this, EventArgs.Empty);
                });
            }
            catch (Exception ex)
            {
                if (RefreshDataFailed != null)
                    RefreshDataFailed(this, new RefreshPagedDataFailedEventArgs(ex));
            }
        }

        public bool CanChangePage
        {
            get { return _canChangePage; }
            private set
            {
                SetField(ref _canChangePage, value, "CanChangePage");
            }
        }

        public bool IsPageChanging
        {
            get { return _isPageChanging; }
            private set
            {
                SetField(ref _isPageChanging, value, "IsPageChanging");
            }
        }

        public int ItemCount
        {
            get { return _itemCount; }
            private set
            {
                SetField(ref _itemCount, value, "ItemCount");
            }
        }

        public event EventHandler<EventArgs> PageChanged;

        public event EventHandler<PageChangingEventArgs> PageChanging;

        public int PageIndex
        {
            get { return _pageIndex; }
            private set
            {
                SetField(ref _pageIndex, value, "PageIndex");
            }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                SetField(ref _pageSize, value, "PageSize");
            }
        }

        public int TotalItemCount
        {
            get { return _totalItemCount; }
            private set
            {
                SetField(ref _totalItemCount, value, "TotalItemCount");
            }
        }

        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((double)TotalItemCount / (double)PageSize);
            }
        }

        /// <summary>
        /// Updates the items exposed by this view with the given data
        /// </summary>
        private void DataReceived(PagedDataResponse<T> response)
        {
            Execute.OnUIThread(() =>
                {
                    TotalItemCount = ItemCount = response.TotalItemCount;
                });
            this.ClearItems();

            foreach (var item in response.Items)
                this.Add(item);
        }

        /// <summary>
        /// Sets the given field, raising a PropertyChanged event
        /// </summary>
        private void SetField<T>(ref T field, T value, string propertyName)
        {
            if (object.Equals(field, value))
                return;

            field = value;
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
