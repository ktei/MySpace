using System;
using MongoDB.Bson.Serialization.Attributes;
using LiteApp.MySpace.Web.Logging;

namespace LiteApp.MySpace.Web.DataAccess.Mongo.PO
{
    public class LogEntryPO
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        public string Detail { get; set; }

        public DateTime CreatedOn { get; set; }

        public LogLevel Level { get; set; }
    }
}