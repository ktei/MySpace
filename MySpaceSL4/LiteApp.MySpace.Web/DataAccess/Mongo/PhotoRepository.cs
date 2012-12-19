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
            Database.GetCollection<Photo>(Collections.Photos).EnsureIndex("AlbumId");
            Database.GetCollection<Photo>(Collections.Photos).EnsureIndex("CreatedOn");
            Database.GetCollection<PhotoComment>(Collections.PhotoComments).EnsureIndex("PhotoId");
            Database.GetCollection<PhotoComment>(Collections.PhotoComments).EnsureIndex("AlbumId");
            Database.GetCollection<PhotoComment>(Collections.PhotoComments).EnsureIndex("CreatedOn");
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

        public void DeletePhotos(IEnumerable<string> photoIds, string albumId)
        {
            ObjectId albumObjectId;
            if (!ObjectId.TryParse(albumId, out albumObjectId))
                return;
            foreach (var photoId in photoIds)
            {
                ObjectId photoObjectId;
                
                if (ObjectId.TryParse(photoId, out photoObjectId))
                {
                    Database.GetCollection<Photo>(Collections.Photos).Remove(
                        Query.And(
                        Query.EQ("_id", photoObjectId), 
                        Query.EQ("AlbumId", albumObjectId)));
                }
            }
        }

        public int GetTotalPhotoCount(string albumId)
        {
            ObjectId albumObjectId;
            if (ObjectId.TryParse(albumId, out albumObjectId))
            {
                return Convert.ToInt32(Database.GetCollection<Photo>(Collections.Photos).Count(Query.EQ("AlbumId", albumObjectId)));
            }
            return 0;
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
            ObjectId commentObjectId;
            if (ObjectId.TryParse(commentId, out commentObjectId))
            {
                Database.GetCollection<PhotoComment>(Collections.PhotoComments).Remove(Query.EQ("_id", commentObjectId));
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

        public PhotoComment GetCommentById(string commentId)
        {
            ObjectId commentObjectId;
            if (ObjectId.TryParse(commentId, out commentObjectId))
            {
                return Database.GetCollection<PhotoComment>(Collections.PhotoComments).FindOneByIdAs<PhotoComment>(commentObjectId);
            }
            return null;
        }


        public void UpdateComment(string newContents, string commentId)
        {
            ObjectId commentObjectId;
            if (ObjectId.TryParse(commentId, out commentObjectId))
            {
                Database.GetCollection<PhotoComment>(Collections.PhotoComments).Update(Query.EQ("_id", commentObjectId), Update.Set("Contents", newContents));
            }
        }
    }
}