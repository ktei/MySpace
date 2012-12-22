using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using LiteApp.MySpace.Web.DataAccess.Mongo.PO;
using LiteApp.MySpace.Entities;

namespace LiteApp.MySpace.Web.DataAccess.Mongo
{
    public class PhotoRepository : BaseRepository, IPhotoRepository
    {
        public PhotoRepository()
        {
            Database.GetCollection<PhotoPO>(Collections.Photos).EnsureIndex("AlbumId");
            Database.GetCollection<PhotoPO>(Collections.Photos).EnsureIndex("CreatedBy");
            Database.GetCollection<PhotoPO>(Collections.Photos).EnsureIndex("CreatedOn");
            Database.GetCollection<PhotoCommentPO>(Collections.PhotoComments).EnsureIndex("PhotoId");
            Database.GetCollection<PhotoCommentPO>(Collections.PhotoComments).EnsureIndex("AlbumId");
            Database.GetCollection<PhotoCommentPO>(Collections.PhotoComments).EnsureIndex("CreatedBy");
            Database.GetCollection<PhotoCommentPO>(Collections.PhotoComments).EnsureIndex("CreatedOn");
        }

        public IEnumerable<Photo> GetPagedPhotos(int pageIndex, int pageSize, string albumId)
        {
            ObjectId albumObjectId;
            if (ObjectId.TryParse(albumId, out albumObjectId))
            {
                var cursor = Database.GetCollection<PhotoPO>(Collections.Photos).FindAs<PhotoPO>(Query.EQ("AlbumId", albumObjectId));
                cursor.SetSortOrder(SortBy.Descending("CreatedOn")).SetSkip(pageIndex * pageSize).SetLimit(pageSize);
                foreach (var photoPO in cursor)
                {
                    yield return photoPO.ToPhoto();
                }
            }
        }

        public void SavePhoto(Photo photo)
        {
            PhotoPO photoPO = photo.ToPhotoPO();
            photoPO.CreatedOn = DateTime.Now;
            Database.GetCollection<PhotoPO>(Collections.Photos).Save(photoPO);
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
                    Database.GetCollection<PhotoPO>(Collections.Photos).Remove(
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
                return Convert.ToInt32(Database.GetCollection<PhotoPO>(Collections.Photos).Count(Query.EQ("AlbumId", albumObjectId)));
            }
            return 0;
        }

        public IEnumerable<PhotoComment> GetComments(string photoId)
        {
            ObjectId photoObjectId;
            if (ObjectId.TryParse(photoId, out photoObjectId))
            {
                var query = Database.GetCollection<PhotoCommentPO>(Collections.PhotoComments).
                    FindAs<PhotoCommentPO>(Query.EQ("PhotoId", photoObjectId)).SetSortOrder(SortBy.Descending("CreatedOn"));
                foreach (var commentPO in query)
                {
                    yield return commentPO.ToPhotoComment();
                }
            }
        }

        public void SaveComment(PhotoComment comment)
        {
            PhotoCommentPO commentPO = comment.ToPhotoCommentPO();
            commentPO.CreatedOn = DateTime.Now;
            Database.GetCollection<PhotoCommentPO>(Collections.PhotoComments).Save(commentPO);
        }

        public void DeleteComment(string commentId, string createdBy = null)
        {
            ObjectId commentObjectId;
            if (ObjectId.TryParse(commentId, out commentObjectId))
            {
                if (createdBy == null)
                {
                    Database.GetCollection<PhotoCommentPO>(Collections.PhotoComments).Remove(Query.EQ("_id", commentObjectId));
                }
                else
                {
                    Database.GetCollection<PhotoCommentPO>(Collections.PhotoComments).Remove(Query.And(
                        Query.EQ("_id", commentObjectId), 
                        Query.EQ("CreatedBy", createdBy)));
                }
            }
        }

        public int GetCommentCount(string photoId)
        {
            ObjectId photoObjectId;
            if (ObjectId.TryParse(photoId, out photoObjectId))
            {
                return Convert.ToInt32(Database.GetCollection<PhotoCommentPO>(Collections.PhotoComments).Count(Query.EQ("PhotoId", photoObjectId)));
            }
            return 0;
        }

        public PhotoComment GetCommentById(string commentId)
        {
            ObjectId commentObjectId;
            if (ObjectId.TryParse(commentId, out commentObjectId))
            {
                return Database.GetCollection<PhotoCommentPO>(Collections.PhotoComments).FindOneByIdAs<PhotoComment>(commentObjectId);
            }
            return null;
        }


        public void UpdateDescription(string description, string photoId, string createdBy = null)
        {
            ObjectId photoObjectId;
            if (ObjectId.TryParse(photoId, out photoObjectId))
            {
                if (createdBy == null)
                {
                    Database.GetCollection<PhotoCommentPO>(Collections.Photos).Update(Query.EQ("_id", photoObjectId), Update.Set("Description", description));
                }
                else
                {
                    Database.GetCollection<PhotoCommentPO>(Collections.Photos).Update(Query.And(
                        Query.EQ("_id", photoObjectId), 
                        Query.EQ("CreatedBy", createdBy)),
                        Update.Set("Description", description));
                }
            }
        }


        public Photo FindPhotoById(string photoId)
        {
            ObjectId photoObjectId;
            if (ObjectId.TryParse(photoId, out photoObjectId))
            {
                return Database.GetCollection<PhotoComment>(Collections.Photos).FindOneByIdAs<Photo>(photoObjectId);
            }
            return null;
        }
    }
}