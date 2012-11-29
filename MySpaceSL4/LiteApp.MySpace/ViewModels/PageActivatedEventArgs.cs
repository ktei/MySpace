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

namespace LiteApp.MySpace.ViewModels
{
    public class PageActivatedEventArgs : EventArgs
    {
        public PageActivatedEventArgs(string activePageName)
        {
            ActivePageName = activePageName;
        }

        public string ActivePageName { get; private set; }
    }
}
