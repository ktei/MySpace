using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading;
using System.Web;
using System.Web.Security;
using Ninject.Web;
using LiteApp.MySpace.Web.Shared;
using Ninject;
using LiteApp.MySpace.Web.ErrorHandling;
using LiteApp.MySpace.Web.Helpers;
using LiteApp.MySpace.Web.Security;
using System.IO;
using System.Web.UI;
using System.Security.Policy;
using LiteApp.MySpace.Web.Logging;
using System;

namespace LiteApp.MySpace.Web.Services
{
    [ServiceContract(Namespace = "")]
    [SilverlightFaultBehavior]
    [ErrorHandlingBehavior(typeof(GenericErrorHandler))]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class SecurityService : WebServiceBase
    {
        public SecurityService()
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
        }

        [Inject]
        public IPhotoUploadTicketPool PhotoUploadTicketPool { get; set; }

        [Inject]
        public ICryptography Cryptography { get; set; }

        [Inject]
        public ILogger Logger { get; set; }

        [OperationContract]
        public SignInStaus SignIn(string userName, string password, bool isPersistent)
        {
            SignInStaus result = SignInStaus.Success;
            try
            {
                if (Membership.ValidateUser(userName, password))
                {
                    FormsAuthentication.SetAuthCookie(userName, isPersistent);
                }
                else
                {
                    // Check if this user is approved or not
                    var user = Membership.GetUser(userName);
                    if (user != null && !user.IsApproved)
                    {
                        // User just registered. Account not approved yet
                        result = SignInStaus.Inactive;
                    }
                    else
                    {
                        result = SignInStaus.WrongCredentials;
                    }
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
            MembershipCreateStatus status = MembershipCreateStatus.Success;
            try
            {
                // Just create the user but don't approve it for now.
                // We need to send the user an email and let him activate
                // his account.
                Membership.CreateUser(userName, password, email, null, null, false, null, out status);
                var signUpStatus = MapToSignUpStatus(status);
                if (signUpStatus == SignUpStatus.Success)
                {
                    var activationTicket = Cryptography.Encrypt(userName);
                    EmailHelper.Send("Welcome to Rui's Space", GetActivationEmailContent(activationTicket), email);
                }
                return signUpStatus;
            }
            catch
            {
                return SignUpStatus.ServerError;
            }

        }

        [OperationContract]
        public ActivationUserResult ActivateUser(string activationTicket)
        {
            ActivationUserResult result = new ActivationUserResult() { Status = ActivationUserStatus.UserNotFound, UserName = null };
            try
            {
                var decodedTicket = HttpUtility.UrlDecode(activationTicket);
                var userName = Cryptography.Decrypt(decodedTicket);
                result.UserName = userName;
                var user = Membership.GetUser(userName);
                if (user != null)
                {
                    if (user.IsApproved)
                    {
                        result.Status = ActivationUserStatus.AlreadyActivated;
                    }
                    else
                    {
                        user.IsApproved = true;
                        Membership.UpdateUser(user);
                        FormsAuthentication.SetAuthCookie(userName, false);
                        result.Status = ActivationUserStatus.Success;
                    }
                }
                else
                {
                    result.Status = ActivationUserStatus.UserNotFound;
                }
            }
            catch (Exception ex)
            {
                result.Status = ActivationUserStatus.ServerError;
                Logger.Error(ex.ToString());
            }
            return result;
        }

        [OperationContract]
        [FaultContract(typeof(ServerFault))]
        public string RequestPhotoUploadTicket(string requestToken)
        {
            string result = null;
            // Only logged-in user can get the ticket
            ServiceSupport.AuthorizeAndExecute(() =>
                {
                    result = PhotoUploadTicketPool.GenerateTicket(requestToken);
                });
            return result;
        }

        [OperationContract]
        public string IsAuthenticated()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
                return HttpContext.Current.User.Identity.Name;
            return string.Empty;
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

        private string GetActivationEmailContent(string ticket)
        {
            StringWriter sw = new StringWriter();
            using (HtmlTextWriter writer = new HtmlTextWriter(sw))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Html);
                writer.RenderBeginTag(HtmlTextWriterTag.Body);

                writer.RenderBeginTag(HtmlTextWriterTag.H3);
                writer.Write("We want to thank you for your sign-up!");
                writer.RenderEndTag();

                writer.Write("Your account has been created. The last thing you need to do is just click ");
                string encodedTicket = HttpUtility.UrlEncode(ticket);
                string host = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
                string url = "http://" + host + "?activationTicket=" + encodedTicket;

                writer.AddAttribute(HtmlTextWriterAttribute.Href, url);

                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write("HERE");
                writer.RenderEndTag();

                writer.Write(" to activate your account.");

                writer.RenderEndTag();
                writer.RenderEndTag();
            }
            return sw.ToString();
        }
    }
}
