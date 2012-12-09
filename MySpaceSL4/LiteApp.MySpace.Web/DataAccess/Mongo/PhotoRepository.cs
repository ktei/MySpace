using System;
using System.Collections.Generic;
using LiteApp.MySpace.Web.Entities;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace LiteApp.MySpace.Web.DataAccess.Mongo
{
    public class PhotoRepository : BaseRepository, IPhotoRepository
    {
        public IEnumerable<Photo> GetPagedPhotos(int pageIndex, int pageSize)
        {
            Database.GetCollection<Photo>(Collections.Albums).EnsureIndex("CreatedOn");
            var cursor = Database.GetCollection<Album>(Collections.Albums).FindAllAs<Photo>();
            cursor.SetSortOrder(SortBy.Descending("CreatedOn")).SetSkip(pageIndex * pageSize).SetLimit(pageSize);
            return cursor;
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