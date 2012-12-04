using System;

namespace LiteApp.MySpace.Framework
{
    /// <summary>
    /// Defines a source of data that can be paged.
    /// </summary>
    public interface IPagedDataSource<TDataType>
    {
        /// <summary>
        /// Asynchronously returns the data for the given page
        /// </summary>
        void FetchData(int pageIndex, Action<PagedDataResponse<TDataType>> responseCallback);

        int PageSize { get; set; }
    }
}
