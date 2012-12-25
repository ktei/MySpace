using System;
using System.Windows;
using LiteApp.MySpace.Services.Security;
using System.ComponentModel;
using Caliburn.Micro;
using LiteApp.MySpace.ViewModels.Message;
using LiteApp.MySpace.Assets.Strings;

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

        public void ActivateUser(string activationTicket, Action<ActivationResult> completeAction)
        {
            if (completeAction == null)
                throw new ArgumentNullException("completeAction");
            try
            {
                SecurityServiceClient svc = new SecurityServiceClient();
                svc.ActivateUserCompleted += (sender, e) =>
                {
                    ActivationResult result = null;
                    if (e.Error != null)
                    {
                        result = new ActivationResult(null, false, ErrorStrings.GenericServerErrorMessage);
                    }
                    else
                    {
                        if (e.Result.Status == ActivationUserStatus.Success)
                        {
                            result = new ActivationResult(e.Result.UserName);
                            User = new IdentityImpl(e.Result.UserName);
                            IsAuthenticated = true;
                        }
                        else if (e.Result.Status == ActivationUserStatus.AlreadyActivated)
                        {
                            result = new ActivationResult(null, false, string.Format(ErrorStrings.UserAlreadyActivatedMessage, e.Result.UserName));
                        }
                        else if (e.Result.Status == ActivationUserStatus.UserNotFound)
                        {
                            result = new ActivationResult(null, false, ErrorStrings.ActivationFailed_InvalidTicket);
                        }
                        else
                        {
                            result = new ActivationResult(null, false, ErrorStrings.ActivationFailed_ServerError);
                        }
                    }
                    completeAction(result);
                };
                svc.ActivateUserAsync(activationTicket);
            }
            catch
            {
                completeAction(new ActivationResult(null, false, ErrorStrings.GenericErrorMessage));
                IsBusy = false;
            }
        }

        public void LoadUser()
        {
            try
            {
                string signedInUser = string.Empty;
                var svc = new SecurityServiceClient();
                svc.IsAuthenticatedCompleted += (sneder, e) =>
                    {
                        if (e.Error == null)
                        {
                            signedInUser = e.Result;
                            if (!string.IsNullOrEmpty(signedInUser))
                            {
                                User = new IdentityImpl(signedInUser);
                                IsAuthenticated = true;
                            }
                        }
                    };
                svc.IsAuthenticatedAsync();
            }
            catch
            {
            }
        }

        public void SignIn(string userName, string password, bool isPersistent, Action<SignInResult> completeAction)
        {
            if (completeAction == null)
                throw new ArgumentNullException("completeAction");
            try
            {
                IsBusy = true;
                Status = AppStrings.SigningInMessage;
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
                        result = new SignInResult(null, false, ErrorStrings.InvalidCredentialsMessage);
                    }
                    else if (status == SignInStaus.Inactive)
                    {
                        result = new SignInResult(null, false, ErrorStrings.InactiveAccountMessage);
                    }
                    else
                    {
                        result = new SignInResult(null, false, ErrorStrings.GenericServerErrorMessage);
                    }
                    completeAction(result);
                    IsBusy = false;
                };
                svc.SignInAsync(userName, password, isPersistent);
            }
            catch
            {
                completeAction(new SignInResult(null, false, ErrorStrings.GenericErrorMessage));
                IsBusy = false;
            }
        }

        public void SignOut(System.Action completeAction = null)
        {
            try
            {
                IsBusy = true;
                Status = AppStrings.SigningOutMessage;
                var svc = new SecurityServiceClient();
                svc.SignOutCompleted += (sender, e) =>
                    {
                        User = IdentityImpl.Empty;
                        IsAuthenticated = false;
                        if (completeAction != null)
                            completeAction();
                        IsBusy = false;
                        IoC.Get<IEventAggregator>().Publish(new SignedOutMessage());
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
