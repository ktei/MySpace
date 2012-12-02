using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web.Security;

namespace LiteApp.MySpace.Web.Services
{
    [ServiceContract(Namespace = "")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class PhotoService
    {
        [OperationContract]
        public void UploadPhoto(byte[] stream)
        {
            
            
        }
    }
}
