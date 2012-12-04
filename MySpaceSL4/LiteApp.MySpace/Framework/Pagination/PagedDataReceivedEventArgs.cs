using System;

namespace LiteApp.MySpace.Framework
{
    public class PagedDataReceivedEventArgs<T> : EventArgs
    {
        public PagedDataReceivedEventArgs(PagedDataResponse<T> response)
        {
            Response = response;
        }

        public PagedDataResponse<T> Response { get; private set; }
    }
}
