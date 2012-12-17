using System;
using LiteApp.MySpace.Web.DataAccess.Mongo;

namespace LiteApp.MySpace.Web.Logging.Mongo
{
    public class MongoLogger : BaseRepository, ILogger
    {
        public event EventHandler<LoggingFailedEventArgs> LoggingFailed;

        public MongoLogger()
        {
            Database.GetCollection<LogEntry>(Collections.LogEntries).EnsureIndex("CreatedOn");
            Database.GetCollection<LogEntry>(Collections.LogEntries).EnsureIndex("Level");
        }

        public void Debug(string user, string detail, params object[] parameters)
        {
            try
            {
                var entry = CreateEntry(user, LogLevel.Debug, detail, parameters);
                Database.GetCollection<LogEntry>(Collections.LogEntries).Save(entry);
            }
            catch (Exception ex)
            {
                HandleLoggingFailed(user, detail, LogLevel.Debug, ex);
            }
        }

        public void Information(string user, string detail, params object[] parameters)
        {
            try
            {
                var entry = CreateEntry(user, LogLevel.Information, detail, parameters);
                Database.GetCollection<LogEntry>(Collections.LogEntries).Save(entry);
            }
            catch (Exception ex)
            {
                HandleLoggingFailed(user, detail, LogLevel.Information, ex);
            }
        }

        public void Warning(string user, string detail, params object[] parameters)
        {
            try
            {
                var entry = CreateEntry(user, LogLevel.Warning, detail, parameters);
                Database.GetCollection<LogEntry>(Collections.LogEntries).Save(entry);
            }
            catch (Exception ex)
            {
                HandleLoggingFailed(user, detail, LogLevel.Warning, ex);
            }
        }

        public void Error(string user, string detail, params object[] parameters)
        {
            try
            {
                var entry = CreateEntry(user, LogLevel.Error, detail, parameters);
                Database.GetCollection<LogEntry>(Collections.LogEntries).Save(entry);
            }
            catch (Exception ex)
            {
                HandleLoggingFailed(user, detail, LogLevel.Error, ex);
            }
        }

        public void Fatal(string user, string detail, params object[] parameters)
        {
            try
            {
                var entry = CreateEntry(user, LogLevel.Fatal, detail, parameters);
                Database.GetCollection<LogEntry>(Collections.LogEntries).Save(entry);
            }
            catch (Exception ex)
            {
                HandleLoggingFailed(user, detail, LogLevel.Fatal, ex);
            }
        }

        static LogEntry CreateEntry(string user, LogLevel level, string detail, params object[] parameters)
        {
            LogEntry entry = new LogEntry();
            entry.Level = level;
            entry.Detail = string.Format(detail, parameters);
            entry.CreatedOn = DateTime.Now;
            entry.CreatedBy = user;
            return entry;
        }

        void HandleLoggingFailed(string user, string detail, LogLevel level, Exception actualException)
        {
            if (LoggingFailed != null)
                LoggingFailed(this, new LoggingFailedEventArgs(user, detail, level, actualException));
        }

    }
}