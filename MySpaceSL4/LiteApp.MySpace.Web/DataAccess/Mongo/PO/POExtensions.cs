﻿using LiteApp.MySpace.Entities;

namespace LiteApp.MySpace.Web.DataAccess.Mongo.PO
{

    public static class POExtensions
    {
        #region Album

        public static Album ToAlbum(this AlbumPO po)
        {
            Album result = new Album();
            result.Id = po.Id;
            result.Name = po.Name;
            result.CreatedBy = po.CreatedBy;
            result.CreatedOn = po.CreatedOn;
            result.PhotoCount = po.PhotoCount;
            result.CoverURIs = po.CoverURIs;
            result.Description = po.Description;
            return result;
        }

        public static AlbumPO ToAlbumPO(this Album entity)
        {
            AlbumPO result = new AlbumPO();
            result.Id = entity.Id;
            result.Name = entity.Name;
            result.CreatedBy = entity.CreatedBy;
            result.CreatedOn = entity.CreatedOn;
            result.PhotoCount = entity.PhotoCount;
            result.CoverURIs = entity.CoverURIs;
            result.Description = entity.Description;
            return result;
        }

        #endregion // Album

        #region Photo

        public static Photo ToPhoto(this PhotoPO po)
        {
            Photo result = new Photo();
            result.Id = po.Id;
            result.AlbumId = po.Id;
            result.CreatedBy = po.CreatedBy;
            result.CreatedOn = po.CreatedOn;
            result.Description = po.Description;
            result.PhotoURI = po.PhotoURI;
            result.ThumbURI = po.ThumbURI;
            result.CommentCount = po.CommentCount;
            return result;
        }

        public static PhotoPO ToPhotoPO(this Photo entity)
        {
            PhotoPO result = new PhotoPO();
            result.Id = entity.Id;
            result.AlbumId = entity.Id;
            result.CreatedBy = entity.CreatedBy;
            result.CreatedOn = entity.CreatedOn;
            result.Description = entity.Description;
            result.PhotoURI = entity.PhotoURI;
            result.ThumbURI = entity.ThumbURI;
            result.CommentCount = entity.CommentCount;
            return result;
        }

        #endregion // Photo

        #region PhotoComment

        public static PhotoComment ToPhotoComment(this PhotoCommentPO po)
        {
            PhotoComment result = new PhotoComment();
            result.Id = po.Id;
            result.PhotoId = po.PhotoId;
            result.AlbumId = po.AlbumId;
            result.Contents = po.Contents;
            result.CreatedBy = po.CreatedBy;
            result.CreatedOn = po.CreatedOn;
            return result;
        }

        public static PhotoCommentPO ToPhotoCommentPO(this PhotoComment entity)
        {
            PhotoCommentPO result = new PhotoCommentPO();

            result.Id = entity.Id;
            result.PhotoId = entity.PhotoId;
            result.AlbumId = entity.AlbumId;
            result.Contents = entity.Contents;
            result.CreatedBy = entity.CreatedBy;
            result.CreatedOn = entity.CreatedOn;
            return result;
        }

        #endregion // PhotoComment

        #region User

        public static User ToUser(this UserPO po)
        {
            User result = new User();
            result.Username = po.Username;
            result.Password = po.Password;
            return result;
        }

        public static UserPO ToUserPO(this User entity)
        {
            UserPO result = new UserPO();
            result.Username = entity.Username;
            result.Password = entity.Password;
            return result;
        }

        #endregion // User
    }
}