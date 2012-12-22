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
        System.Action _cancelButtonAction;

        public UploadPhotoManagerView()
        {
            InitializeComponent();
            this.Loaded += UploadPhotoManagerView_Loaded;
            this.Unloaded += UploadPhotoManagerView_Unloaded;
        }

        void UploadPhotoManagerView_Loaded(object sender, RoutedEventArgs e)
        {
            _model = this.DataContext as UploadPhotoManagerViewModel;
            _model.NoMoreTasks += _model_NoMoreTasks;
            UpdateButtons();
        }


        void UploadPhotoManagerView_Unloaded(object sender, RoutedEventArgs e)
        {
            _model.NoMoreTasks -= _model_NoMoreTasks;
        }

        void _model_NoMoreTasks(object sender, EventArgs e)
        {
            UpdateButtons();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            if (_cancelButtonAction != null)
                _cancelButtonAction();
        }

        private void ChooseFilesButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            if (dlg.ShowDialog() == true)
            {
                _model.StartUpload(dlg.Files);
                UpdateButtons();
            }
        }

        private void UploadPhotoManagerView_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Escape)
            {
                CancelButton.AutomationPeerInvoke();
            }
        }

        void UpdateButtons()
        {
            if (_model.HasArchivedTasks())
            {
                Execute.OnUIThread(() => this.ChooseFilesButton.Content = AppStrings.AddMoreFilesButtonText);
                if (_model.HasMoreTasks())
                {
                    Execute.OnUIThread(() => this.CancelButton.Content = AppStrings.HideButtonText);
                    _cancelButtonAction = () => this.DialogResult = false;
                }
                else
                {
                    Execute.OnUIThread(() =>
                        {
                            this.CancelButton.Content = AppStrings.DoneButtonText;
                            _cancelButtonAction = () =>
                                {
                                    _model.Clear();
                                    this.ChooseFilesButton.Content = AppStrings.ChooseFilesButtonText;
                                    this.CancelButton.Content = AppStrings.CancelButtonText;
                                };
                        });
                }
            }
            else
            {
                Execute.OnUIThread(() =>
                    {
                        this.ChooseFilesButton.Content = AppStrings.ChooseFilesButtonText;
                        this.CancelButton.Content = AppStrings.CancelButtonText;
                    });
            }
        }
    }
}

