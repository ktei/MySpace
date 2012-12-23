using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiteApp.MySpace.Entities;

namespace LiteApp.MySpace.Web.DataAccess
{
    public interface ILogRepository
    {
        IEnumerable<LogEntry> GetPagedLogs(int pageIndex, int pageSize);
        int GetTotalLogCount();
    }
}