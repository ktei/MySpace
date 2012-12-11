using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using LiteApp.MySpace.Web.DataAccess;
using LiteApp.MySpace.Web.Entities;
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
        public void SaveAlbum(Album album)
        {
            AlbumRepository.SaveAlbum(album);
        }

        [OperationContract]
        public void DeleteAlbum(string albumId)
        {
            SharpBoxSupport.DeleteAlbum(albumId);
            AlbumRepository.DeleteAlbum(albumId);
        }

        public string[] UpdateAlbumCover(string albumId, string coverUri)
        {
            return AlbumRepository.UpdateCover(albumId, coverUri);
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

        public void SavePhoto(Photo photo)
        {
            PhotoRepository.SavePhoto(photo);
        }

        #endregion // Photo API
    }
}
