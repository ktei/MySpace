using System;

namespace LiteApp.MySpace.ViewModels
{
    public class PhotoUploadCompletedEventArgs : EventArgs
    {
        public PhotoUploadCompletedEventArgs(string newCoverURIs, Exception error)
        {
            NewCoverURIs = newCoverURIs;
            Error = error;
        }

        public Exception Error { get; private set; }

        public string NewCoverURIs { get; private set; }
    }
}
