using Caliburn.Micro;
using LiteApp.MySpace.Security;

namespace LiteApp.MySpace.ViewModels
{
    public static class ViewModelSupport
    {
        public static void AuthorizeAndExecute(System.Action action)
        {
            if (!SecurityContext.Current.IsAuthenticated)
            {
                SignInViewModel signInModel = new SignInViewModel() { DisplayName = "Sign-in Required", Message = "This operation needs you to sign in first." };
                IoC.Get<IWindowManager>().ShowDialog(signInModel);
            }
            else
            {
                action();
            }
        }
    }
}
