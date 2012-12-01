using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using LiteApp.MySpace.Web.Entities;
using System.Threading;
using System.Web;
using System.Web.Security;

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

        //[OperationContract]
        public SignInStaus SignIn(string userName, string password)
        {
            Thread.Sleep(2500);
            return SignInStaus.Success;
        }

        [OperationContract]
        public SignUpStatus SignUp(string userName, string password, string email)
        {
            Thread.Sleep(5000);
            SignUpStatus result = SignUpStatus.Success;
            try
            {
                //Membership.CreateUser(userName, password, email);
            }
            catch (MembershipCreateUserException ex)
            {
                result = MapToSignUpStatus(ex.StatusCode);
            }
            return result;
        }

        static SignUpStatus MapToSignUpStatus(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.Success: return SignUpStatus.Success;
                case MembershipCreateStatus.InvalidPassword: return SignUpStatus.InvalidPassword;
                case MembershipCreateStatus.InvalidEmail: return SignUpStatus.InvalidEmail;
                case MembershipCreateStatus.DuplicateUserName: return SignUpStatus.DuplicateUserName;
                case MembershipCreateStatus.DuplicateEmail: return SignUpStatus.DuplicateEmail;
                default: return SignUpStatus.OtherError;
            }
        }
    }
}
