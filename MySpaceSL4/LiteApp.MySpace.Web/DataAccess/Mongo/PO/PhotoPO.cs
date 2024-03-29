﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace LiteApp.MySpace.Web.DataAccess.Mongo.PO
{
    public class PhotoPO
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string AlbumId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Description { get; set; }

        public string DownloadURI { get; set; }

        public string PhotoURI { get; set; }

        public string ThumbURI { get; set; }

        public int CommentCount { get; set; }
    }
}