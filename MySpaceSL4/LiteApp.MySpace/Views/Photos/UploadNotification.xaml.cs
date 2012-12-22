using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using LiteApp.MySpace.ViewModels;

namespace LiteApp.MySpace.Views.Photos
{
    public partial class UploadNotification : UserControl
    {
        public UploadNotification()
        {
            InitializeComponent();
            this.DataContext = IoC.Get<UploadPhotoManagerViewModel>();
        }

        private void ViewDetailsLink_Click(object sender, RoutedEventArgs e)
        {
            IoC.Get<IWindowManager>().ShowDialog(this.DataContext);
        }
    }
}
