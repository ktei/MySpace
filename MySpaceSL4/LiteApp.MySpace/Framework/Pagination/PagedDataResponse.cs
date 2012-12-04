using System.Collections.Generic;

namespace LiteApp.MySpace.Framework
{
    /// <summary>
    /// The items returned as a result of a paged data request.
    /// </summary>
    public class PagedDataResponse<TDataType>
    {
        /// <summary>
        /// The items contained within the requested page
        /// </summary>
        public List<TDataType> Items { get; set; }

        /// <summary>
        /// The total count of all available items
        /// </summary>
        public int TotalItemCount { get; set; }
    }
}
