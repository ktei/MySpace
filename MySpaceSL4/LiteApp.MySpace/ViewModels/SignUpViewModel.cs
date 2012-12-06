using System;
using Caliburn.Micro;
using LiteApp.MySpace.Services.Security;
using LiteApp.MySpace.Assets;
using LiteApp.MySpace.Framework;
using LiteApp.Portable.Mvvm.Validation;
using LiteApp.MySpace.Assets.Strings;

namespace LiteApp.MySpace.ViewModels
{
    public class SignUpViewModel : EditableViewModel
    {
        string _userName;
        string _email;
        bool _isBusy;

        public SignUpViewModel()
        {
            DisplayName = "Sign Up";
            RefreshBindingScope = new RefreshBindingScope();
        }

        public event EventHandler SignUpCompleted;

        public RefreshBindingScope RefreshBindingScope { get; set; }

        [RequiredField]
        [LengthConstraint(50)]
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
        [LengthConstraint(6, 25)]
        public string Password
        {
            get { return PasswordAccessor == null ? string.Empty : PasswordAccessor(); }
            set
            {
                IsDirty = true;
                NotifyOfPropertyChange(() => Password);
            }
        }

        [RequiredField]
        public string PasswordConfirmation
        {
            get { return PasswordConfirmationAccessor == null ? string.Empty : PasswordConfirmationAccessor(); }
            set
            {
                IsDirty = true;
                NotifyOfPropertyChange(() => PasswordConfirmation);
            }
        }

        [RequiredField]
        [RegularExpression(".+\\@.+\\..+", ErrorMessageResourceName="InvalidEmail", ErrorMessageResourceType=typeof(ErrorStrings))]
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    NotifyOfPropertyChange(() => Email);
                }
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

        internal Func<string> PasswordConfirmationAccessor { get; set; }

        public override string this[string propertyName]
        {
            get
            {
                if (!IsDirty)
                    return null;
                string error = base[propertyName];
                if (string.IsNullOrEmpty(error) && propertyName == "PasswordConfirmation")
                {
                    if (this.PasswordConfirmation != Password)
                    {
                        return "Passwords do not match.";
                    }
                    return null;
                }
                return error;
            }
        }

        public void SignIn()
        {
            IoC.Get<IWindowManager>().ShowDialog(new SignInViewModel());
        }

        public void SignUp()
        {
            RefreshBindingScope.Scope();
            if (this.Validator.HasErrors)
                return;
            try
            {
                IsBusy = true;
                SecurityServiceClient client = new SecurityServiceClient();

                client.SignUpCompleted += (sender, e) =>
                    {
                        IsBusy = false;
                        
                        if (SignUpCompleted != null)
                            SignUpCompleted(this, EventArgs.Empty);
                    };
                client.SignUpAsync(UserName, Password, Email);
            }
            catch
            {
                IsBusy = false;
            }
        }
    }
}
