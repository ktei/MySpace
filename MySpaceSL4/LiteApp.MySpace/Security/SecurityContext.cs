using System;
using System.Windows;
using LiteApp.MySpace.Services.Security;

namespace LiteApp.MySpace.Security
{
    public class SecurityContext : IApplicationService
    {
        public SecurityContext()
        {
        }

        public static SecurityContext Current { get; private set; }

        public void SignIn(string userName, string password, Action<SignInResult> completeAction)
        {
            if (completeAction == null)
                throw new ArgumentNullException("completeAction");
            try
            {
                var client = new SecurityServiceClient();

                client.SignInCompleted += (sender, e) =>
                {
                    var status = e.Result;
                    SignInResult result = null;
                    if (status == SignInStaus.Success)
                    {
                        User = new IdentityImpl(userName);
                        IsAuthenticated = true;
                        result = new SignInResult(User);
                    }
                    else if (status == SignInStaus.WrongCredentials)
                    {
                        result = new SignInResult(null, false, "Provided credentials are invalid.");
                    }
                    else
                    {
                        result = new SignInResult(null, false, "Server had an error while processing the request. Please try again later.");
                    }
                    completeAction(result);
                };
                client.SignInAsync(userName, password);
            }
            catch
            {
                completeAction(new SignInResult(null, false, "An error occurred while processing the request. Please try again later."));
            }
        }

        public Identity User
        {
            get;
            private set;
        }

        public bool IsAuthenticated
        {
            get;
            private set;
        }

        public void StartService(ApplicationServiceContext context)
        {
            Current = this;
        }

        public void StopService()
        {
            return;
        }

        public class IdentityImpl : Identity
        {
            public IdentityImpl(string name)
            {
                Name = name;
            }

            public string Name
            {
                get;
                private set;
            }
        }
    }
}
