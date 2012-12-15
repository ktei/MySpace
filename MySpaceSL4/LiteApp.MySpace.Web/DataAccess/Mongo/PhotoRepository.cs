using System;
using System.Collections.Generic;
using System.Linq;
using LiteApp.MySpace.Web.Entities;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace LiteApp.MySpace.Web.DataAccess.Mongo
{
    public class PhotoRepository : BaseRepository, IPhotoRepository
    {
        public PhotoRepository()
        {
            Database.GetCollection<Photo>(Collections.Photos).EnsureIndex("CreatedOn");
            Database.GetCollection<PhotoComment>(Collections.PhotoComments).EnsureIndex("CreatedOn");
            Database.GetCollection<PhotoComment>(Collections.PhotoComments).EnsureIndex("PhotoId");
        }

        public IEnumerable<Photo> GetPagedPhotos(int pageIndex, int pageSize, string albumId)
        {
            ObjectId albumObjectId;
            if (ObjectId.TryParse(albumId, out albumObjectId))
            {
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


        public IEnumerable<PhotoComment> GetComments(string photoId)
        {
            ObjectId photoObjectId;
            if (ObjectId.TryParse(photoId, out photoObjectId))
            {
                return Database.GetCollection<PhotoComment>(Collections.PhotoComments).
                    FindAs<PhotoComment>(Query.EQ("PhotoId", photoObjectId)).SetSortOrder(SortBy.Descending("CreatedOn"));
            }
            return Enumerable.Empty<PhotoComment>();
        }

        public void SaveComment(PhotoComment comment)
        {
            comment.CreatedOn = DateTime.Now;
            Database.GetCollection<PhotoComment>(Collections.PhotoComments).Save(comment);
        }

        public void DeleteComment(string commentId)
        {
            ObjectId id;
            if (ObjectId.TryParse(commentId, out id))
            {
                Database.GetCollection<PhotoComment>(Collections.PhotoComments).Remove(Query.EQ("_id", id));
            }
        }

        public int GetCommentCount(string photoId)
        {
            ObjectId photoObjectId;
            if (ObjectId.TryParse(photoId, out photoObjectId))
            {
                return Convert.ToInt32(Database.GetCollection<PhotoComment>(Collections.PhotoComments).Count(Query.EQ("PhotoId", photoObjectId)));
            }
            return 0;
        }
    }
}