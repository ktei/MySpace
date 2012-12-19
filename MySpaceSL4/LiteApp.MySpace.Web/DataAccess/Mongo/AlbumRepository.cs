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


        public Album FindAlbumById(string albumId)
        {
            ObjectId albumObjectId;
            if (ObjectId.TryParse(albumId, out albumObjectId))
            {
                return Database.GetCollection<Album>(Collections.Albums).FindOneByIdAs<Album>(albumObjectId);
            }
            return null;
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
            ObjectId albumObjectId;
            if (ObjectId.TryParse(albumId, out albumObjectId))
            {
                Database.GetCollection<Album>(Collections.Albums).Remove(Query.EQ("_id", albumObjectId));
            }
        }

        public string[] UpdateCover(string albumId, string coverUri)
        {
            ObjectId albumObjectId;
            if (ObjectId.TryParse(albumId, out albumObjectId))
            {
                Database.GetCollection<Album>(Collections.Albums).Update(Query.And(Query.EQ("_id", albumObjectId), Query.Size("CoverURIs", 3)), Update.PopFirst("CoverURIs"));
                Database.GetCollection<Album>(Collections.Albums).Update(Query.EQ("_id", albumObjectId), Update.AddToSet("CoverURIs", coverUri));
            }
            return Database.GetCollection<Album>(Collections.Albums).FindOneAs<Album>(Query.EQ("_id", albumObjectId)).CoverURIs;
        }

        public int GetTotalAlbumCount()
        {
            return Convert.ToInt32(Database.GetCollection<Album>(Collections.Albums).Count());
        }

        public void UpdateCovers(Album album)
        {
            var cursor = Database.GetCollection<Photo>(Collections.Photos).FindAs<Photo>(Query.EQ("AlbumId", ObjectId.Parse(album.Id))).SetLimit(3);
            var covers = cursor.Select(x => x.ThumbURI).ToArray();
            album.CoverURIs = covers;
            Database.GetCollection<Album>(Collections.Albums).Save(album);
        }
    }
}