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
        int _pageSize;
        int _itemCount;
        int _totalItemCount;
        bool _isPageChanging;
        bool _canChangePage;

        public ServerSidePagedCollectionView(IPagedDataSource<T> pagedDataSource)
        {
            _pagedDataSource = pagedDataSource;
            pagedDataSource.PageSize = PageSize;
        }

        public event EventHandler<PagedDataReceivedEventArgs<T>> DataReceived;
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

        /// <summary>
        /// Fetches the data for the given page
        /// </summary>
        private void RefreshData(int newPageIndex)
        {
            try
            {
                // set the pre-fetch state
                CanChangePage = false;
                IsPageChanging = true;
                if (PageChanging != null)
                    PageChanging(this, new PageChangingEventArgs(newPageIndex));

                _pagedDataSource.FetchData(newPageIndex, response =>
                {
                    // process the received data
                    if (DataReceived != null)
                        DataReceived(this, new PagedDataReceivedEventArgs<T>(response));

                    // set the post-fetch state
                    ItemCount = response.Items.Count;
                    TotalItemCount = response.TotalItemCount;
                    PageIndex = newPageIndex;
                    if (PageChanged != null)
                        PageChanged(this, EventArgs.Empty);
                    IsPageChanging = false;
                    CanChangePage = true;
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
                if (_canChangePage != value)
                {
                    _canChangePage = value;
                    NotifyOfPropertyChange("CanChangePage");
                }
            }
        }

        public bool IsPageChanging
        {
            get { return _isPageChanging; }
            private set
            {
                if (_isPageChanging != value)
                {
                    _isPageChanging = value;
                    NotifyOfPropertyChange("IsPageChanging");
                }
            }
        }

        public int ItemCount
        {
            get { return _itemCount; }
            private set
            {
                if (_itemCount != value)
                {
                    _itemCount = value;
                    NotifyOfPropertyChange("ItemCount");
                }
            }
        }

        public event EventHandler<EventArgs> PageChanged;

        public event EventHandler<PageChangingEventArgs> PageChanging;

        public int PageIndex
        {
            get { return _pageIndex; }
            private set
            {
                if (_pageIndex != value)
                {
                    _pageIndex = value;
                    NotifyOfPropertyChange("PageIndex");
                }
            }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    _pagedDataSource.PageSize = value;
                    NotifyOfPropertyChange("PageSize");
                }
            }
        }

        public int TotalItemCount
        {
            get { return _totalItemCount; }
            private set
            {
                if (_totalItemCount != value)
                {
                    _totalItemCount = value;
                    NotifyOfPropertyChange("TotalItemCount");
                }
            }
        }

        public int TotalPages
        {
            get { return TotalItemCount / PageSize + 1; }
        }
    }
}
