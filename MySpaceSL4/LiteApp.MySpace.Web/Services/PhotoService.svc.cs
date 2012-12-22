using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading;
using System.Web;
using LiteApp.MySpace.Web.DataAccess;
using LiteApp.MySpace.Web.FaultHandling;
using LiteApp.MySpace.Web.Helpers;
using Ninject;
using Ninject.Web;
using LiteApp.MySpace.Entities;
using System.ServiceModel.Web;

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
        public PagedResult<Album> GetPagedAlbums(string pageIndex, string pageSize)
        {
            PagedResult<Album> result = new PagedResult<Album>();
            result.Entities = AlbumRepository.GetPagedAlbums(int.Parse(pageIndex), int.Parse(pageSize)).ToList();
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
        public void UpdateAlbum(string name, string description, string albumId)
        {
            ServiceSupport.AuthorizeAndExecute(() =>
                {
                    var album = AlbumRepository.FindAlbumById(albumId);
                    if (album == null)
                    {
                        throw new FaultException<ServerFault>(new ServerFault() { FaultCode = ServerFaultCode.Generic },
                                new FaultReason("No album with Id " + albumId + " was found."));
                    }

                    album.Name = name;
                    album.Description = description;

                    if (HttpContext.Current.IsSuperAdminLoggedIn())
                    {
                        AlbumRepository.SaveAlbum(album);
                    }
                    else
                    {
                        if (!HttpContext.Current.IsUserLoggedIn(album.CreatedBy))
                        {
                            throw new FaultException<ServerFault>(new ServerFault() { FaultCode = ServerFaultCode.NotAuthroized },
                                new FaultReason("Album must only be edited by the author."));
                        }
                        else
                        {
                            AlbumRepository.SaveAlbum(album);
                        }
                    }
                });
        }

        [OperationContract]
        [FaultContract(typeof(ServerFault))]
        public void DeleteAlbum(string albumId)
        {
            ServiceSupport.AuthorizeAndExecute(() =>
                {
                    if (HttpContext.Current.IsSuperAdminLoggedIn())
                    {
                        // TODO: should we consider doing this cloud operation in another thread? How about a background worker?
                        SharpBoxSupport.DeleteAlbum(albumId);
                        AlbumRepository.DeleteAlbum(albumId);
                    }
                    else
                    {
                        var album = AlbumRepository.FindAlbumById(albumId);
                        if (album == null)
                        {
                            throw new FaultException<ServerFault>(new ServerFault() { FaultCode = ServerFaultCode.Generic },
                                    new FaultReason("No album with Id " + albumId + " was found."));
                        }
                        // Only album author can delete photos
                        if (!HttpContext.Current.IsUserLoggedIn(album.CreatedBy))
                        {
                            throw new FaultException<ServerFault>(new ServerFault() { FaultCode = ServerFaultCode.NotAuthroized },
                                new FaultReason("Album must only be deleted by the author."));
                        }
                        else
                        {
                            // Delete photos by selected IDs and album ID
                            SharpBoxSupport.DeleteAlbum(albumId);
                            AlbumRepository.DeleteAlbum(albumId);
                        }
                    }
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
        [FaultContract(typeof(ServerFault))]
        public void UpdateDescription(string description, string photoId)
        {
            ServiceSupport.AuthorizeAndExecute(() =>
            {
                if (HttpContext.Current.IsSuperAdminLoggedIn())
                {
                    PhotoRepository.UpdateDescription(description, photoId);
                }
                else
                {
                    PhotoRepository.UpdateDescription(description, photoId, HttpContext.Current.User.Identity.Name);
                }
            });
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
                        PhotoRepository.DeleteComment(commentId, HttpContext.Current.User.Identity.Name);
                    }
                });
        }

        #endregion // Photo API
    }
}
