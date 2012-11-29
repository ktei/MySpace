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
    public class SignInOperation
    {
        public SignInOperation(Identity user, bool success = true, string error = null)
        {
        }

        public bool Success { get; private set; }

        public Identity User { get; private set; }

        public string Error { get; private set; }
    }
}
