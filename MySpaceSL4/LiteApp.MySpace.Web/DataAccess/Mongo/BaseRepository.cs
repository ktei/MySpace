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
        public static readonly string DatabaseName = "MySapace";
#if DEBUG
        static MongoClient client = new MongoClient(GetMongoDbConnectionString());
#endif

        protected static MongoDatabase Database
        {
            get
            {
#if DEBUG
                return client.GetServer().GetDatabase(DatabaseName);
#else
                return MongoDatabase.Create(GetMongoDbConnectionString());
#endif
            }
        }

        protected static string GetMongoDbConnectionString()
        {
#if DEBUG
            return ConfigurationManager.ConnectionStrings["MySpace_Mongo_Dev"].ConnectionString;
#else
            return ConfigurationManager.ConnectionStrings["MySpace_Mongo_Release"].ConnectionString;
#endif
        }

        public static class Collections
        {
            public static readonly string Users = "Users";
            public static readonly string Albums = "Albums";
            public static readonly string Photos = "Photos";
            public static readonly string LogEntries = "LogEntries";
        }
    }
}