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
using LiteApp.MySpace.Security;

namespace LiteApp.MySpace.ViewModels
{
    public static class ViewModelSupport
    {
        public static bool VerifyAuthentication()
        {
            if (!SecurityContext.Current.IsAuthenticated)
            {
                SignInViewModel signInModel = new SignInViewModel() { DisplayName = "Sign-in Required", Message = "This operation needs you to sign in first." };
                IoC.Get<IWindowManager>().ShowDialog(signInModel);
                return false;
            }
            return true;
        }
    }
}
