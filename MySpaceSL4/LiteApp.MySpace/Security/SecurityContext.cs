using System;
using System.Windows;
using LiteApp.MySpace.Services.Security;
using System.ComponentModel;

namespace LiteApp.MySpace.Security
{
    public class SecurityContext : INotifyPropertyChanged, IApplicationService
    {
        bool _isAuthenticated;
        bool _isBusy;
        string _status;
        Identity _user;

        public SecurityContext()
        {
            User = IdentityImpl.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static SecurityContext Current { get; private set; }

        public bool IsSuperAdminSignedIn()
        {
            return this.IsUserSignedIn("ktei");
        }

        public bool IsUserSignedIn(string userName)
        {
            return IsAuthenticated && User.Name == userName;
        }

        public void SignIn(string userName, string password, Action<SignInResult> completeAction)
        {
            if (completeAction == null)
                throw new ArgumentNullException("completeAction");
            try
            {
                IsBusy = true;
                Status = "Signing you in...";
                var svc = new SecurityServiceClient();

                svc.SignInCompleted += (sender, e) =>
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
                        result = new SignInResult(null, false, "Provided credentials are invalid. Please check your user name and password.");
                    }
                    else
                    {
                        result = new SignInResult(null, false, "Server had an error while processing the request. Please try again later.");
                    }
                    completeAction(result);
                    IsBusy = false;
                };
                svc.SignInAsync(userName, password);
            }
            catch
            {
                completeAction(new SignInResult(null, false, "An error occurred while processing the request. Please try again later."));
                IsBusy = false;
            }
        }

        public void SignOut(Action completeAction = null)
        {
            try
            {
                IsBusy = true;
                Status = "Signing you out...";
                var svc = new SecurityServiceClient();
                svc.SignOutCompleted += (sender, e) =>
                    {
                        User = IdentityImpl.Empty;
                        IsAuthenticated = false;
                        if (completeAction != null)
                            completeAction();
                        IsBusy = false;
                    };
                svc.SignOutAsync();
            }
            catch
            {
                if (completeAction != null)
                    completeAction();
                IsBusy = false;
            }
        }

        public Identity User
        {
            get { return _user; }
            private set
            {
                if (_user != value)
                {
                    _user = value;
                    RaisePropertyChanged("User");
                }
            }
        }

        public bool IsAuthenticated
        {
            get { return _isAuthenticated; }
            private set
            {
                if (_isAuthenticated != value)
                {
                    _isAuthenticated = value;
                    RaisePropertyChanged("IsAuthenticated");
                }
            }
        }

        public string Status
        {
            get { return _status; }
            private set
            {
                if (_status != value)
                {
                    _status = value;
                    RaisePropertyChanged("Status");
                }
            }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            private set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    RaisePropertyChanged("IsBusy");
                }
            }
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
            static readonly IdentityImpl _empty = new IdentityImpl(string.Empty);

            public IdentityImpl(string name)
            {
                Name = name;
            }

            public string Name
            {
                get;
                private set;
            }

            public static IdentityImpl Empty
            {
                get { return _empty; }
            }
        }

        void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
