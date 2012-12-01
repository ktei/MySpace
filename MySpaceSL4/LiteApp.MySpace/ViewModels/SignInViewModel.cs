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

namespace LiteApp.MySpace.ViewModels
{
    public class SignInViewModel : Screen
    {
        public SignInViewModel()
        {
            DisplayName = "Sign In";
        }

        public void Register()
        {
            IoC.Get<IWindowManager>().ShowDialog(new SignUpViewModel());
        }
    }
}
