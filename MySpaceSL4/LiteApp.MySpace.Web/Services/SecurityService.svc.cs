using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading;
using System.Web;
using System.Web.Security;
using Ninject.Web;

namespace LiteApp.MySpace.Web.Services
{
    [ServiceContract(Namespace = "")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class SecurityService : WebServiceBase
    {
        [OperationContract]
        public SignInStaus SignIn(string userName, string password)
        {
            SignInStaus result = SignInStaus.Success;
            try
            {
                if (Membership.ValidateUser(userName, password))
                {
                    FormsAuthentication.SetAuthCookie(userName, false); 
                }
                else
                {
                    result = SignInStaus.WrongCredentials;
                }
            }
            catch
            {
                // TODO: log error
                result = SignInStaus.ServerError;
            }
            return result;
        }

        [OperationContract]
        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        [OperationContract]
        public SignUpStatus SignUp(string userName, string password, string email)
        {
            SignUpStatus result = SignUpStatus.Success;
            try
            {
                Membership.CreateUser(userName, password, email);
            }
            catch (MembershipCreateUserException ex)
            {
                // TODO: log error
                result = MapToSignUpStatus(ex.StatusCode);
            }
            return result;
        }

        [OperationContract]
        public bool IsAuthenticated()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
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
                default: return SignUpStatus.ServerError;
            }
        }
    }
}
