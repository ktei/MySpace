using System;
using MongoDB.Bson.Serialization.Attributes;

namespace LiteApp.MySpace.Web.Logging.Mongo
{
    public class LogEntry
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public string Detail { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public LogLevel Level { get; set; }
    }
}