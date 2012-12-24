using System;
using LiteApp.MySpace.Web.DataAccess.Mongo;
using LiteApp.MySpace.Web.Logging;
using LiteApp.MySpace.Web.DataAccess.Mongo.PO;
using MongoDB.Driver.Builders;
using System.Collections.Generic;

namespace LiteApp.MySpace.Web.DataAccess.Mongo
{
    public class LogRepository : BaseRepository, ILogRepository, ILogger
    {
        public event EventHandler<LoggingFailedEventArgs> LoggingFailed;

        public LogRepository()
        {
            Database.GetCollection<LogEntryPO>(Collections.LogEntries).EnsureIndex("CreatedOn");
            Database.GetCollection<LogEntryPO>(Collections.LogEntries).EnsureIndex("Level");
        }

        public void Debug(string detail, params object[] parameters)
        {
            try
            {
                var entry = CreateEntry(Logging.LogLevel.Debug, detail, parameters);
                Database.GetCollection<LogEntryPO>(Collections.LogEntries).Save(entry);
            }
            catch (Exception ex)
            {
                HandleLoggingFailed(detail, LogLevel.Debug, ex);
            }
        }

        public void Information(string detail, params object[] parameters)
        {
            try
            {
                var entry = CreateEntry(LogLevel.Information, detail, parameters);
                Database.GetCollection<LogEntryPO>(Collections.LogEntries).Save(entry);
            }
            catch (Exception ex)
            {
                HandleLoggingFailed(detail, LogLevel.Information, ex);
            }
        }

        public void Warning(string detail, params object[] parameters)
        {
            try
            {
                var entry = CreateEntry(LogLevel.Warning, detail, parameters);
                Database.GetCollection<LogEntryPO>(Collections.LogEntries).Save(entry);
            }
            catch (Exception ex)
            {
                HandleLoggingFailed(detail, LogLevel.Warning, ex);
            }
        }

        public void Error(string detail, params object[] parameters)
        {
            try
            {
                var entry = CreateEntry(LogLevel.Error, detail, parameters);
                Database.GetCollection<LogEntryPO>(Collections.LogEntries).Save(entry);
            }
            catch (Exception ex)
            {
                HandleLoggingFailed(detail, LogLevel.Error, ex);
            }
        }

        public void Fatal(string detail, params object[] parameters)
        {
            try
            {
                var entry = CreateEntry(LogLevel.Fatal, detail, parameters);
                Database.GetCollection<LogEntryPO>(Collections.LogEntries).Save(entry);
            }
            catch (Exception ex)
            {
                HandleLoggingFailed(detail, LogLevel.Fatal, ex);
            }
        }

        public IEnumerable<Entities.LogEntry> GetPagedLogs(int pageIndex, int pageSize)
        {
            var cursor = Database.GetCollection<LogEntryPO>(Collections.LogEntries).FindAllAs<LogEntryPO>();
            cursor.SetSortOrder(SortBy.Descending("CreatedOn")).SetSkip(pageIndex * pageSize).SetLimit(pageSize);
            foreach (var entry in cursor)
            {
                yield return entry.ToLogEntry();
            }
        }

        public int GetTotalLogCount()
        {
            return Convert.ToInt32(Database.GetCollection<LogEntryPO>(Collections.LogEntries).Count());
        }

        static LogEntryPO CreateEntry(LogLevel level, string detail, params object[] parameters)
        {
            LogEntryPO entry = new LogEntryPO();
            entry.Level = level;
            entry.Detail = string.Format(detail, parameters);
            entry.CreatedOn = DateTime.Now.ToUniversalTime();
            return entry;
        }

        void HandleLoggingFailed(string detail, LogLevel level, Exception actualException)
        {
            if (LoggingFailed != null)
                LoggingFailed(this, new LoggingFailedEventArgs(detail, level, actualException));
        }
    }
}