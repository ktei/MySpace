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
using System.ComponentModel.Composition;
using LiteApp.MySpace.Framework;
using Caliburn.Micro;
using LiteApp.MySpace.Security;

namespace LiteApp.MySpace.ViewModels
{
    [Export(typeof(IPage))]
    [PageMetadata("Photos")]
    public class PhotosViewModel : Screen, IPage
    {
        public PhotosViewModel()
        {
            
        }

        public string Name
        {
            get { return "Photos"; }
        }

        public void UploadPhoto()
        {
            IoC.Get<IWindowManager>().ShowDialog(new UploadPhotoViewModel());
        }
    }
}
