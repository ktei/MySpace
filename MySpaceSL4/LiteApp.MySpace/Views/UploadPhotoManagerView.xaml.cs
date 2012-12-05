using System;
using System.Windows;
using System.Windows.Controls;
using LiteApp.MySpace.ViewModels;
using LiteApp.MySpace.Views.Extensions;

namespace LiteApp.MySpace.Views
{
    public partial class UploadPhotoManagerView : ChildWindow
    {
        UploadPhotoManagerViewModel _model;

        public UploadPhotoManagerView()
        {
            InitializeComponent();
            this.Loaded += UploadPhotoManagerView_Loaded;
        }

        void UploadPhotoManagerView_Loaded(object sender, RoutedEventArgs e)
        {
            _model = this.DataContext as UploadPhotoManagerViewModel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ChooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = false;
            if (dlg.ShowDialog() == true)
            {
                _model.StartUpload(dlg.File);
            }
        }

        private void UploadPhotoManagerView_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                CancelButton.AutomationPeerInvoke();
            }
        }
    }
}

