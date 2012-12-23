using System;

namespace LiteApp.MySpace.ViewModels
{
    public class AsyncOperationCompletedEventArgs : EventArgs
    {
        public AsyncOperationCompletedEventArgs(Exception error = null)
        {
            Error = error;
        }

        public Exception Error { get; private set; }
    }
}
