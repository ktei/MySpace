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
            result.TotalItemCount = PhotoRepository.GetTotalPhotoCount(albumId);
            return result;
        }

        // TODO: this method should return the new covers for requested album so that
        // client side can update its UI
        [OperationContract]
        [FaultContract(typeof(ServerFault))]
        public string[] DeletePhotos(DeletePhotoParameters[] photos, string albumId)
        {
            string[] albumCovers = new string[] { };
            ServiceSupport.AuthorizeAndExecute(() =>
                {
                    var album = AlbumRepository.FindAlbumById(albumId);
                    if (album == null)
                    {
                        throw new FaultException<ServerFault>(new ServerFault() { FaultCode = ServerFaultCode.Generic },
                                new FaultReason("No album with Id " + albumId + " was found."));
                    }

                    var photoIds = photos.Select(x => x.PhotoId);
                    
                    // TODO: should we consider doing this cloud operation in another thread? How about a background worker?
                    var storage = SharpBoxSupport.OpenDropBoxStorage();
                    foreach (var photoFile in photos.Select(x => x.FileName))
                    {
                        storage.DeletePhoto(photoFile, albumId);
                    }
                    
                    if (HttpContext.Current.IsSuperAdminLoggedIn())
                    {
                        PhotoRepository.DeletePhotos(photoIds, albumId);
                        AlbumRepository.UpdateCovers(album);
                    }
                    else
                    {
                        // Only album author can delete photos
                        if (!HttpContext.Current.IsUserLoggedIn(album.CreatedBy))
                        {
                            throw new FaultException<ServerFault>(new ServerFault() { FaultCode = ServerFaultCode.NotAuthroized },
                                new FaultReason("Photos must only be deleted by the author of the album they belong to."));
                        }
                        else
                        {
                            // Delete photos by selected IDs and album ID
                            PhotoRepository.DeletePhotos(photoIds, albumId);
                            AlbumRepository.UpdateCovers(album);
                        }
                    }
                    albumCovers = album.CoverURIs;
                });
            return albumCovers;
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
                    if (HttpContext.Current.IsSuperAdminLoggedIn())
                    {
                        PhotoRepository.DeleteComment(commentId);
                    }
                    else
                    {
                        var comment = PhotoRepository.GetCommentById(commentId);
                        if (comment != null)
                        {
                            if (HttpContext.Current.IsUserLoggedIn(comment.CreatedBy))
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
