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

namespace LiteApp.MySpace.Security
{
    public interface IAuthenticationProvider
    {
        void SignIn(string userName, string password, Action<SignInOperation> completeAction);
    }
}
