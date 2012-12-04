using System;

namespace LiteApp.MySpace.Framework
{
    public class RefreshPagedDataFailedEventArgs : EventArgs
    {
        public RefreshPagedDataFailedEventArgs(Exception error)
        {
            Error = error;
        }

        public Exception Error { get; private set; }
    }
}
