using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System;

namespace LiteApp.MySpace.Web.DataAccess.Mongo.PO
{
    public class AlbumPO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int PhotoCount { get; set; }

        public string[] CoverURIs { get; set; }

        public string Description { get; set; }
    }
}