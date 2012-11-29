using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using LiteApp.MySpace.Web.Entities;
using System.Threading;
using System.Web;

namespace LiteApp.MySpace.Web.Services
{
    [ServiceContract(Namespace = "")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class SecurityService
    {
        public SecurityService()
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
        }

        [OperationContract]
        public SignInResult SignIn(string userName, string password)
        {
            Thread.Sleep(2500);
            return new SignInResult(SignInResult.StatusCode.Success, new User() { Username = "ktei" });
        }
    }

    public class SignInResult
    {
        public SignInResult(StatusCode status, User user)
        {
            this.Status = status;
            this.User = user;
        }

        public User User { get; private set; }

        public StatusCode Status { get; private set; }

        public enum StatusCode
        {
            Success = 0,
            WrongCredentials
        }
    }
}
