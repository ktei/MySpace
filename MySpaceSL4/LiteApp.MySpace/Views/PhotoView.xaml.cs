using System.Windows;
using System.Windows.Controls;
using LiteApp.MySpace.ViewModels;
using System.Windows.Input;
using LiteApp.MySpace.Views.Helpers;
using Caliburn.Micro;

namespace LiteApp.MySpace.Views
{
    public partial class PhotoView : ChildWindow
    {
        public PhotoView()
        {
            InitializeComponent();
            this.Unloaded += PhotoView_Unloaded;
        }

        void PhotoView_Unloaded(object sender, RoutedEventArgs e)
        {
            ((PhotoViewModel)this.DataContext).IsLoadingPhoto = true;
        }

        private void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            ((PhotoViewModel)this.DataContext).IsLoadingPhoto = false;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void PhotoView_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CloseButton.AutomationPeerInvoke();
            }
        }

        private void SignInLink_Click(object sender, RoutedEventArgs e)
        {
            SignInViewModel model = new SignInViewModel();
            IoC.Get<IWindowManager>().ShowDialog(model);
        }
    }
}

