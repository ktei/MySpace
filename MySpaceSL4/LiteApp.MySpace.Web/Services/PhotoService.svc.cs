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
        public IEnumerable<Album> GetAllAlbums()
        {
            return AlbumRepository.GetAllAlbums();
        }

        [OperationContract]
        public void SaveAlbum(Album album)
        {
            AlbumRepository.SaveAlbum(album);
        }
    }
}
