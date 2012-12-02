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
        static MongoClient client = new MongoClient(GetMongoDbConnectionString());

        protected static MongoDatabase Database
        {
            get
            {
                return client.GetServer().GetDatabase(DatabaseName);
            }
        }

        protected static string GetMongoDbConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["MySpace_Mongo_Dev"].ConnectionString;
        }

        public static class Collections
        {
            public static readonly string Users = "Users";
            public static readonly string Albums = "Albums";
        }
    }
}