using System;
using LiteApp.MySpace.Entities;

namespace LiteApp.MySpace.ViewModels
{
    public class LogEntryViewModel
    {
        DateTime _createdOn;

        public string Id { get; set; }

        public LogLevel Level { get; set; }

        public string Detail { get; set; }

        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set
            {
                if (_createdOn != value)
                {
                    _createdOn = value.ToLocalDateTime();
                }
            }
        }
    }
}
