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
using LiteApp.MySpace.Framework;
using System.ComponentModel.Composition;
using Caliburn.Micro;

namespace LiteApp.MySpace.ViewModels
{
    [Export(typeof(IPage))]
    [PageMetadata("Home")]
    public class HomeViewModel : Screen, IPage
    {
        public string Name
        {
            get { return "Home"; }
        }
    }
}
