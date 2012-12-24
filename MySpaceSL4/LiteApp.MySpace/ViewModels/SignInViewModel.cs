using System;
using System.Windows;
using Caliburn.Micro;
using LiteApp.MySpace.Framework;
using LiteApp.MySpace.Security;
using LiteApp.Portable.Mvvm.Validation;
using LiteApp.MySpace.Assets.Strings;

namespace LiteApp.MySpace.ViewModels
{
    public class SignInViewModel : EditableViewModel
    {
        string _userName;
        bool _isBusy;
        string _message;

        public SignInViewModel()
        {
            DisplayName = AppStrings.SignInWindowTitle;
            RefreshBindingScope = new RefreshBindingScope();
        }

        public event EventHandler SignInSucceeded;

        public RefreshBindingScope RefreshBindingScope { get; set; }

        [RequiredField]
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    IsDirty = true;
                    NotifyOfPropertyChange(() => UserName);
                }
            }
        }

        [RequiredField]
        public string Password
        {
            get { return PasswordAccessor == null ? string.Empty : PasswordAccessor(); }
            set
            {
                IsDirty = true;
                NotifyOfPropertyChange(() => Password);
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    NotifyOfPropertyChange(() => Message);
                }
            }
        }

        public bool KeepSignedIn
        {
            get;
            set;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    NotifyOfPropertyChange(() => IsBusy);
                }
            }
        }

        internal Func<string> PasswordAccessor { get; set; }

        public void SignUp()
        {
            IoC.Get<IWindowManager>().ShowDialog(new SignUpViewModel());
        }

        public void SignIn()
        {
            RefreshBindingScope.Scope();
            if (this.Validator.HasErrors)
                return;
            IsBusy = true;
            SecurityContext.Current.SignIn(UserName, Password, KeepSignedIn, result =>
            {
                if (!result.Success)
                {
                    var messageBox = new MessageBoxViewModel()
                    {
                        MessageLevel = ViewModels.MessageLevel.Exclamation,
                        Buttons = MessageBoxButtons.OK,
                        Header = AppStrings.SignInFailedMessageHeader,
                        Message = result.Error,
                        DisplayName = AppStrings.SignInWindowTitle
                    };
                    IoC.Get<IWindowManager>().ShowDialog(messageBox);
                }
                else
                {
                    if (SignInSucceeded != null)
                        SignInSucceeded(this, EventArgs.Empty);
                }

                IsBusy = false;
            });
        }
    }
}
