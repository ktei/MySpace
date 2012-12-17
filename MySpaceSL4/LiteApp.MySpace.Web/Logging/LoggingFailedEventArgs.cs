using System;

namespace LiteApp.MySpace.Web.Logging
{
    public class LoggingFailedEventArgs : EventArgs
    {
        public LoggingFailedEventArgs(string user, string detail, LogLevel level, Exception actualException)
        {
            User = user;
            Detail = detail;
            Level = level;
            ActualException = actualException;
        }

        public string User { get; private set; }
        public string Detail { get; private set; }
        public LogLevel Level { get; private set; }
        public Exception ActualException { get; private set; }
    }
}