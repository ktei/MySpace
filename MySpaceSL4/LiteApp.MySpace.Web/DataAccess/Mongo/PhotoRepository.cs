using System;
using System.Collections.Generic;
using LiteApp.MySpace.Web.Entities;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using System.Linq;

namespace LiteApp.MySpace.Web.DataAccess.Mongo
{
    public class PhotoRepository : BaseRepository, IPhotoRepository
    {
        public IEnumerable<Photo> GetPagedPhotos(int pageIndex, int pageSize, string albumId)
        {
            ObjectId albumObjectId;
            if (ObjectId.TryParse(albumId, out albumObjectId))
            {
                Database.GetCollection<Photo>(Collections.Photos).EnsureIndex("CreatedOn");
                var cursor = Database.GetCollection<Photo>(Collections.Photos).FindAs<Photo>(Query.EQ("AlbumId", albumObjectId));
                cursor.SetSortOrder(SortBy.Descending("CreatedOn")).SetSkip(pageIndex * pageSize).SetLimit(pageSize);
                return cursor;
            }
            return Enumerable.Empty<Photo>();
        }

        public void SavePhoto(Photo photo)
        {
            photo.CreatedOn = DateTime.Now;
            Database.GetCollection<Photo>(Collections.Photos).Save(photo);
        }

        public void DeletePhoto(string photoId)
        {
            ObjectId id;
            if (ObjectId.TryParse(photoId, out id))
            {
                Database.GetCollection<Photo>(Collections.Photos).Remove(Query.EQ("_id", id));
            }
        }


        public int GetTotalPhotoCount()
        {
            return Convert.ToInt32(Database.GetCollection<Photo>(Collections.Photos).Count());
        }
    }
}