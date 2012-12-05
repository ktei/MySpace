﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LiteApp.MySpace.Web.Entities
{
    public class Album
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public int PhotoCount { get; set; }

        public string CoverUri { get; set; }

        public string Description { get; set; }
    }
}