using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiteApp.MySpace.Web.Entities;
using MongoDB.Driver.Builders;
using MongoDB.Bson;

namespace LiteApp.MySpace.Web.DataAccess.Mongo
{
    public class AlbumRepository : BaseRepository, IAlbumRepository
    {
        public IEnumerable<Album> GetPagedAlbums(int pageIndex, int pageSize)
        {
            Database.GetCollection<Album>(Collections.Albums).EnsureIndex("Name");
            var cursor = Database.GetCollection<Album>(Collections.Albums).FindAllAs<Album>();
            cursor.SetSortOrder(SortBy.Ascending("Name")).SetSkip(pageIndex * pageSize).SetLimit(pageSize);
            return cursor;
        }

        public void SaveAlbum(Album album)
        {
            Database.GetCollection<Album>(Collections.Albums).Save(album);
            
        }

        public void DeleteAlbum(string albumId)
        {
            ObjectId id;
            if (ObjectId.TryParse(albumId, out id))
            {
                Database.GetCollection<Album>(Collections.Albums).Remove(Query.EQ("_id", id));
            }
        }

        public int GetTotalAlbumCount()
        {
            return Convert.ToInt32(Database.GetCollection<Album>(Collections.Albums).Count());
        }
    }
}