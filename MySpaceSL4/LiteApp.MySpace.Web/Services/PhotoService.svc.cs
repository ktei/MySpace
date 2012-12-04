using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Security;
using System.Collections.Generic;
using LiteApp.MySpace.Web.Entities;
using LiteApp.MySpace.Web.DataAccess;
using Ninject;
using Ninject.Web;
using System.Threading;

namespace LiteApp.MySpace.Web.Services
{
    [ServiceContract(Namespace = "")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class PhotoService : WebServiceBase
    {
        [Inject]
        public IAlbumRepository AlbumRepository { get; set; }

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
            AlbumRepository.DeleteAlbum(albumId);
        }
    }
}
