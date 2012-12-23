using System;
using LiteApp.MySpace.Entities;

namespace LiteApp.MySpace.ViewModels
{
    public class LogEntryViewModel
    {
        public string Id { get; set; }

        public LogLevel Level { get; set; }

        public string Detail { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
