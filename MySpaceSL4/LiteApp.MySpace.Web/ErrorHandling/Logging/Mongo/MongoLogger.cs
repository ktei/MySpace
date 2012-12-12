using System;
using LiteApp.MySpace.Web.DataAccess.Mongo;

namespace LiteApp.MySpace.Web.ErrorHandling.Logging.Mongo
{
    public class MongoLogger : BaseRepository, ILogger
    {
        public MongoLogger()
        {
            Database.GetCollection<LogEntry>(Collections.LogEntries).EnsureIndex("CreatedOn");
            Database.GetCollection<LogEntry>(Collections.LogEntries).EnsureIndex("Level");
        }

        public void Debug(string user, string detail, params object[] parameters)
        {
            var entry = CreateEntry(LogLevel.Debug, detail, parameters);
            Database.GetCollection<LogEntry>(Collections.LogEntries).Save(entry);
        }

        public void Information(string user, string detail, params object[] parameters)
        {
            var entry = CreateEntry(LogLevel.Information, detail, parameters);
            Database.GetCollection<LogEntry>(Collections.LogEntries).Save(entry);
        }

        public void Warning(string user, string detail, params object[] parameters)
        {
            var entry = CreateEntry(LogLevel.Warning, detail, parameters);
            Database.GetCollection<LogEntry>(Collections.LogEntries).Save(entry);
        }

        public void Error(string user, string detail, params object[] parameters)
        {
            var entry = CreateEntry(LogLevel.Error, detail, parameters);
            Database.GetCollection<LogEntry>(Collections.LogEntries).Save(entry);
        }

        public void Fatal(string user, string detail, params object[] parameters)
        {
            var entry = CreateEntry(LogLevel.Fatal, detail, parameters);
            Database.GetCollection<LogEntry>(Collections.LogEntries).Save(entry);
        }

        static LogEntry CreateEntry(LogLevel level, string detail, params object[] parameters)
        {
            LogEntry entry = new LogEntry();
            entry.Level = level;
            entry.Detail = string.Format(detail, parameters);
            entry.CreatedOn = DateTime.Now;
            return entry;
        }
    }
}