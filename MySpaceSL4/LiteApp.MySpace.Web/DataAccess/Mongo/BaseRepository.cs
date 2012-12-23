using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using System.Configuration;

namespace LiteApp.MySpace.Web.DataAccess.Mongo
{
    public abstract class BaseRepository
    {
        protected static MongoDatabase Database
        {
            get
            {
                return MongoDatabase.Create(GetMongoDbConnectionString());
            }
        }

        protected static string GetMongoDbConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MySpace_Mongo"].ConnectionString;
        }

        public static class Collections
        {
            public static readonly string Users = "Users";
            public static readonly string Albums = "Albums";
            public static readonly string Photos = "Photos";
            public static readonly string PhotoComments = "PhotoComments";
            public static readonly string LogEntries = "LogEntries";
        }
    }
}