using Caliburn.Micro;
using LiteApp.MySpace.Security;
using LiteApp.MySpace.Assets.Strings;
using System;

namespace LiteApp.MySpace.ViewModels
{
    public static class ViewModelSupport
    {
        public static void ShowSignInDialog()
        {
            SignInViewModel model = new SignInViewModel();
            IoC.Get<IWindowManager>().ShowDialog(model);
        }

        public static DateTime ToLocalDateTime(this DateTime utc)
        {
            return DateTime.SpecifyKind(utc, DateTimeKind.Utc).ToLocalTime();
        }
    }
}
