using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading;
using System.Web;
using LiteApp.MySpace.Web.DataAccess;
using LiteApp.MySpace.Web.Entities;
using LiteApp.MySpace.Web.FaultHandling;
using LiteApp.MySpace.Web.Helpers;
using Ninject;
using Ninject.Web;

namespace LiteApp.MySpace.Web.Services
{
    [ServiceContract(Namespace = "")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class PhotoService : WebServiceBase
    {
        public PhotoService()
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
        }

        [Inject]
        public IAlbumRepository AlbumRepository { get; set; }

        [Inject]
        public IPhotoRepository PhotoRepository { get; set; }

        #region Album API

        [OperationContract]
        public PagedResult<Album> GetPagedAlbums(int pageIndex, int pageSize)
        {
            PagedResult<Album> result = new PagedResult<Album>();
            result.Entities = AlbumRepository.GetPagedAlbums(pageIndex, pageSize).ToList();
            result.TotalItemCount = AlbumRepository.GetTotalAlbumCount();
            return result;
        }

        [OperationContract]
        [FaultContract(typeof(ServerFault))]
        public void SaveAlbum(Album album)
        {
            ServiceSupport.AuthorizeAndExecute(() => AlbumRepository.SaveAlbum(album));
        }

        [OperationContract]
        [FaultContract(typeof(ServerFault))]
        public void DeleteAlbum(string albumId)
        {
            ServiceSupport.AuthorizeAndExecute(() =>
                {
                    SharpBoxSupport.DeleteAlbum(albumId);
                    AlbumRepository.DeleteAlbum(albumId);
                });
        }

        #endregion // Album API

        #region Photo API

        [OperationContract]
        public PagedResult<Photo> GetPagedPhotos(int pageIndex, int pageSize, string albumId)
        {
            PagedResult<Photo> result = new PagedResult<Photo>();
            result.Entities = PhotoRepository.GetPagedPhotos(pageIndex, pageSize, albumId).ToList();
            result.TotalItemCount = PhotoRepository.GetTotalPhotoCount();
            return result;
        }

        [OperationContract]
        public IEnumerable<PhotoComment> GetComments(string photoId)
        {
            return PhotoRepository.GetComments(photoId);
        }

        [OperationContract]
        [FaultContract(typeof(ServerFault))]
        public PhotoComment SaveComment(PhotoComment comment)
        {
            Thread.Sleep(2000);
            ServiceSupport.AuthorizeAndExecute(() =>
                {
                    PhotoRepository.SaveComment(comment);
                });
            return comment;
        }

        [OperationContract]
        [FaultContract(typeof(ServerFault))]
        public void DeleteComment(string commentId)
        {
            ServiceSupport.AuthorizeAndExecute(() => 
                {
                    if (HttpContext.Current.User.Identity.Name == "ktei")
                    {
                        PhotoRepository.DeleteComment(commentId);
                    }
                    else
                    {
                        var comment = PhotoRepository.GetCommentById(commentId);
                        if (comment != null)
                        {
                            if (comment.CreatedBy != HttpContext.Current.User.Identity.Name)
                            {
                                throw new FaultException<ServerFault>(new ServerFault() { FaultCode = ServerFaultCode.NotAuthroized },
                                    new FaultReason("Comment must only be deleted by its author"));
                            }
                            PhotoRepository.DeleteComment(commentId);
                        }
                    }
                });
        }

        #endregion // Photo API
    }
}
