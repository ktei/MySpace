using System;
using System.Windows;

namespace LiteApp.MySpace.Security
{
    public class SecurityContext : IApplicationService
    {
        public SecurityContext()
        {
            AuthenticationProvider = new AuthenticationProvider();
        }

        public static SecurityContext Current { get; private set; }

        public IAuthenticationProvider AuthenticationProvider { get; set; }

        public void SignIn(string userName, string password)
        {
            if (this.AuthenticationProvider == null)
            {
                throw new Exception("AuthenticationService is not set.");
            }
            AuthenticationProvider.SignIn(userName, password, result =>
                {
                    if (result.Success)
                    {
                        User = result.User;
                        IsAuthenticated = true;
                    }
                });
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
    }
}
