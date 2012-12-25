using System.Collections.Generic;
using LiteApp.MySpace.Entities;
using LiteApp.MySpace.Web.DataAccess.Mongo.PO;

namespace LiteApp.MySpace.Web.DataAccess
{
    public interface IPhotoRepository
    {
        IEnumerable<Photo> GetPagedPhotos(int pageIndex, int pageSize, string albumId);
        Photo FindPhotoById(string photoId);
        void CreatePhoto(Photo photo);
        void DeletePhotos(IEnumerable<string> photoIds, string albumId);
        int GetTotalPhotoCount(string albumId);
        void UpdateDescription(string description, string photoId, string createdBy = null);

        IEnumerable<PhotoComment> GetComments(string photoId);
        PhotoComment GetCommentById(string commentId);
        PhotoComment CreateComment(PhotoComment comment);
        void DeleteComment(string commentId, string createdBy = null);
        int GetCommentCount(string photoId);
    }
}