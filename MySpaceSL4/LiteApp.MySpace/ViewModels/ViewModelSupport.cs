using Caliburn.Micro;
using LiteApp.MySpace.Security;
using LiteApp.MySpace.Assets.Strings;

namespace LiteApp.MySpace.ViewModels
{
    public static class ViewModelSupport
    {
        public static void AuthorizeAndExecute(System.Action action)
        {
            if (!SecurityContext.Current.IsAuthenticated)
            {
                SignInViewModel signInModel = new SignInViewModel() { DisplayName = AppStrings.SignInRequiredMessageHeader, Message = AppStrings.OperationNeedsSignInMessage };
                IoC.Get<IWindowManager>().ShowDialog(signInModel);
            }
            else
            {
                action();
            }
        }
    }
}
