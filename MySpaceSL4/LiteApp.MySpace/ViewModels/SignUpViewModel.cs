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
            DisplayName = AppStrings.SignUpWindowTitle;
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
                        return ErrorStrings.PasswordsDoNotMatch;
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
                        if (e.Error != null)
                        {
                            e.Error.Handle();
                        }
                        else
                        {
                            HandleSignUpResult(e.Result);
                        }
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

        static void HandleSignUpResult(SignUpStatus status)
        {
            if (status == SignUpStatus.Success)
            {
                Execute.OnUIThread(() =>
                {
                    MessageBoxViewModel message = new MessageBoxViewModel();
                    message.DisplayName = AppStrings.SignUpWindowTitle;
                    message.Header = AppStrings.SucceededMessageHeader;
                    message.MessageLevel = MessageLevel.Success;
                    message.Message = AppStrings.SignUpSuccessMessage;
                    IoC.Get<IWindowManager>().ShowDialog(message);
                });
            }
            else
            {
                var errorMessage = GetSignUpErrorMessage(status);
                Execute.OnUIThread(() =>
                {
                    MessageBoxViewModel message = new MessageBoxViewModel();
                    message.DisplayName = AppStrings.SignUpWindowTitle;
                    message.Header = AppStrings.FailedMessageHeader;
                    message.MessageLevel = MessageLevel.Exclamation;
                    message.Message = errorMessage;
                    IoC.Get<IWindowManager>().ShowDialog(message);
                });
            }
        }

        static string GetSignUpErrorMessage(SignUpStatus status)
        {
            switch (status)
            {
                case SignUpStatus.ServerError: return ErrorStrings.GenericErrorMessage;
                case SignUpStatus.DuplicateUserName: return ErrorStrings.SignUpFailed_DuplicateUserName;
                case SignUpStatus.DuplicateEmail: return ErrorStrings.SignUpFailed_DuplicateEmail;
                case SignUpStatus.InvalidEmail: return ErrorStrings.InvalidEmail;
                case SignUpStatus.InvalidPassword: return ErrorStrings.SignUpFailed_InvalidPassword;
                default: return string.Empty;
            }
        }
    }
}
