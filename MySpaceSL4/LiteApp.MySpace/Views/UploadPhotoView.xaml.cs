using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using LiteApp.MySpace.ViewModels;

namespace LiteApp.MySpace.Views
{
    public partial class UploadPhotoView : ChildWindow
    {
        UploadPhotoViewModel _model;

        public UploadPhotoView()
        {
            InitializeComponent();
            this.Loaded += UploadPhotoView_Loaded;
        }

        void UploadPhotoView_Loaded(object sender, RoutedEventArgs e)
        {
            _model = this.DataContext as UploadPhotoViewModel;
        }

        void _model_UploadStarted(object sender, EventArgs e)
        {
            this.DialogResult = true;
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
                this.DialogResult = true;
            }
        }
    }
}

