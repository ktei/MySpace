using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using LiteApp.MySpace.ViewModels;
using LiteApp.MySpace.Assets.Strings;

namespace LiteApp.MySpace.Views
{
    public partial class MessageBoxView : ChildWindow
    {
        MessageBoxViewModel _model;

        public MessageBoxView()
        {
            InitializeComponent();
            this.Loaded += MessageBoxView_Loaded;
            this.Unloaded += MessageBoxView_Unloaded;
        }

        void MessageBoxView_Loaded(object sender, RoutedEventArgs e)
        {
            _model = this.DataContext as MessageBoxViewModel;
            if (_model == null)
                throw new Exception("Mode of type of MessageBoxViewModel is needed");
            ApplyButtons(_model.Buttons);
            _model.PropertyChanged += _model_PropertyChanged;
        }

        void _model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Buttons")
            {
                ApplyButtons(_model.Buttons);
            }
            else if (e.PropertyName == "PositiveButtonText" || e.PropertyName == "NegativeButtonText" || e.PropertyName == "CancelButtonText")
            {
                if (_model.Buttons == MessageBoxButtons.Custom)
                {
                    ApplyButtons(_model.Buttons);
                }
            }
        }

        void MessageBoxView_Unloaded(object sender, RoutedEventArgs e)
        {
            if (_model != null)
                _model.PropertyChanged -= _model_PropertyChanged;
        }

        private void PositiveButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void NegativeButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void ApplyButtons(MessageBoxButtons buttons)
        {
            HideAllButtons();
            if (buttons == MessageBoxButtons.OK)
            {
                PositiveButton.Visibility = System.Windows.Visibility.Visible;
                PositiveButton.Content = AppStrings.OKButtonText;
            }
            else if (buttons == MessageBoxButtons.OKCancel)
            {
                PositiveButton.Visibility = System.Windows.Visibility.Visible;
                NegativeButton.Visibility = System.Windows.Visibility.Visible;
                PositiveButton.Content = AppStrings.OKButtonText;
                NegativeButton.Content = AppStrings.CancelButtonText;
            }
            else if (buttons == MessageBoxButtons.YesNo)
            {
                PositiveButton.Visibility = System.Windows.Visibility.Visible;
                NegativeButton.Visibility = System.Windows.Visibility.Visible;
                PositiveButton.Content = AppStrings.YesButtonText;
                NegativeButton.Content = AppStrings.NoButtonText;
            }
            else if (buttons == MessageBoxButtons.Custom)
            {
                PositiveButton.Visibility = string.IsNullOrEmpty(_model.PositiveButtonText) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
                NegativeButton.Visibility = string.IsNullOrEmpty(_model.NegativeButtonText) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
                CancelButton.Visibility = string.IsNullOrEmpty(_model.CancelButtonText) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
                PositiveButton.Content = _model.PositiveButtonText;
                NegativeButton.Content = _model.NegativeButtonText;
                CancelButton.Content = _model.CancelButtonText;
            }
        }

        void HideAllButtons()
        {
            PositiveButton.Visibility = System.Windows.Visibility.Collapsed;
            CancelButton.Visibility = System.Windows.Visibility.Collapsed;
            NegativeButton.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}

