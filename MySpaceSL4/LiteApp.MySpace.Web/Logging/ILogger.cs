using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteApp.MySpace.Web.Logging
{
    public interface ILogger
    {
        void Debug(string user, string detail, params object[] parameters);
        void Information(string user, string detail, params object[] parameters);
        void Warning(string user, string detail, params object[] parameters);
        void Error(string user, string detail, params object[] parameters);
        void Fatal(string user, string detail, params object[] parameters);
    }
}