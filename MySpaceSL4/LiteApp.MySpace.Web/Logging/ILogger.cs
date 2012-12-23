
using System;
namespace LiteApp.MySpace.Web.Logging
{
    public interface ILogger
    {
        void Debug(string detail, params object[] parameters);
        void Information(string detail, params object[] parameters);
        void Warning(string detail, params object[] parameters);
        void Error(string detail, params object[] parameters);
        void Fatal(string detail, params object[] parameters);
        event EventHandler<LoggingFailedEventArgs> LoggingFailed;
    }
}