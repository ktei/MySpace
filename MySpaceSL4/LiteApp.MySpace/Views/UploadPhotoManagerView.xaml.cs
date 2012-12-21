using System;
using System.Windows;
using System.Windows.Controls;
using LiteApp.MySpace.ViewModels;
using LiteApp.MySpace.Views.Helpers;
using LiteApp.MySpace.Assets.Strings;
using Caliburn.Micro;

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
            if (_model.HasMoreTasks())
            {
                MessageBoxViewModel message = new MessageBoxViewModel()
                {
                    Buttons = MessageBoxButtons.YesNo,
                    MessageLevel = MessageLevel.Exclamation,
                    Header = AppStrings.CancelUploadMessageHeader,
                    Message = AppStrings.ConfirmCancelUploadPhotoMessage,
                    DisplayName = AppStrings.UploadPhotoWindowTitle
                };

                message.Closed += message_Closed;
                IoC.Get<IWindowManager>().ShowDialog(message);
            }
            else
            {
                this.DialogResult = false;
            }
        }

        void message_Closed(object sender, EventArgs e)
        {
            MessageBoxViewModel message = (MessageBoxViewModel)sender;
            if (message.Result == ViewModels.MessageBoxResult.Positive)
            {
                message.Closed -= message_Closed;
                _model.Clear();
                this.DialogResult = false;
            }
        }

        private void ChooseFilesButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == true)
            {
                _model.StartUpload(dlg.Files);
                ChooseFilesButton.Content = AppStrings.AddMoreFilesButtonText;
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

