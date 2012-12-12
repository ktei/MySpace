using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiteApp.MySpace.Web.Entities;

namespace LiteApp.MySpace.Web.DataAccess
{
    public interface IPhotoRepository
    {
        IEnumerable<Photo> GetPagedPhotos(int pageIndex, int pageSize, string albumId);
        void SavePhoto(Photo photo);
        void DeletePhoto(string photoId);
        int GetTotalPhotoCount();

        IEnumerable<PhotoComment> GetComments(string photoId);
        void SaveComment(PhotoComment comment);
        void DeleteComment(string commentId);
        int GetCommentCount(string photoId);
    }
}