using Caliburn.Micro;
using LiteApp.MySpace.Security;
using LiteApp.MySpace.Assets.Strings;

namespace LiteApp.MySpace.ViewModels
{
    public static class ViewModelSupport
    {
        public static void ShowSignInDialog()
        {
            SignInViewModel model = new SignInViewModel();
            IoC.Get<IWindowManager>().ShowDialog(model);
        }
    }
}
