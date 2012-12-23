using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteApp.MySpace.Entities
{
    public class LogEntry
    {
        public string Id { get; set; }

        public string Detail { get; set; }

        public DateTime CreatedOn { get; set; }

        public LogLevel Level { get; set; }
    }

    public enum LogLevel
    {
        Debug = 0,
        Information,
        Warning,
        Error,
        Fatal
    }
}
