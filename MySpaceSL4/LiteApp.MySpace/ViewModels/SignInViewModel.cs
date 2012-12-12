using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using LiteApp.MySpace.Services.Security;
using LiteApp.MySpace.Security;
using LiteApp.MySpace.Framework;
using LiteApp.Portable.Mvvm.Validation;

namespace LiteApp.MySpace.ViewModels
{
    public class SignInViewModel : EditableViewModel
    {
        string _userName;
        bool _isBusy;

        public SignInViewModel()
        {
            DisplayName = "Sign In";
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
            SecurityContext.Current.SignIn(UserName, Password, result =>
            {
                if (!result.Success)
                {
                    MessageBox.Show(result.Error);
                }
                else
                {
                    IsBusy = false;
                    if (SignInSucceeded != null)
                        SignInSucceeded(this, EventArgs.Empty);
                }
            });
        }
    }
}
