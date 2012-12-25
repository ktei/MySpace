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
        PhotoViewModel _model;

        public PhotoView()
        {
            InitializeComponent();
            this.Loaded += PhotoView_Loaded;
            this.Unloaded += PhotoView_Unloaded;
        }

        void PhotoView_Loaded(object sender, RoutedEventArgs e)
        {
            _model = (PhotoViewModel)this.DataContext;
            _model.RequestClose += _model_RequestClose;
        }

        void PhotoView_Unloaded(object sender, RoutedEventArgs e)
        {
            _model.IsLoadingPhoto = true;
            _model.RequestClose -= _model_RequestClose;
        }

        void _model_RequestClose(object sender, System.EventArgs e)
        {
            this.DialogResult = false;
        }

        private void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            _model.IsLoadingPhoto = false;
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

