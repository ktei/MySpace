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
        public AlbumRepository()
        {
            Database.GetCollection<Album>(Collections.Albums).EnsureIndex("CreatedOn");
        }

        public IEnumerable<Album> GetPagedAlbums(int pageIndex, int pageSize)
        {   
            var cursor = Database.GetCollection<Album>(Collections.Albums).FindAllAs<Album>();
            cursor.SetSortOrder(SortBy.Descending("CreatedOn")).SetSkip(pageIndex * pageSize).SetLimit(pageSize);
            return cursor;
        }

        public void SaveAlbum(Album album)
        {
            if (album.CoverURIs == null)
                album.CoverURIs = new string[] { };
            album.CreatedOn = DateTime.Now;
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

        public string[] UpdateCover(string albumId, string coverUri)
        {
            ObjectId id;
            if (ObjectId.TryParse(albumId, out id))
            {
                Database.GetCollection<Album>(Collections.Albums).Update(Query.And(Query.EQ("_id", id), Query.Size("CoverURIs", 3)), Update.PopFirst("CoverURIs"));
                Database.GetCollection<Album>(Collections.Albums).Update(Query.EQ("_id", id), Update.AddToSet("CoverURIs", coverUri));
            }
            return Database.GetCollection<Album>(Collections.Albums).FindOneAs<Album>(Query.EQ("_id", id)).CoverURIs;
        }

        public int GetTotalAlbumCount()
        {
            return Convert.ToInt32(Database.GetCollection<Album>(Collections.Albums).Count());
        }
    }
}