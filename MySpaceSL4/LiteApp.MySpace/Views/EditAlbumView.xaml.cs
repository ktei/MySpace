using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LiteApp.MySpace.ViewModels;
using LiteApp.MySpace.Views.Helpers;

namespace LiteApp.MySpace.Views
{
    public partial class EditAlbumView : ChildWindow
    {
        EditAlbumViewModel _model;

        public EditAlbumView()
        {
            InitializeComponent();
            this.Loaded += EditAlbumView_Loaded;
            this.Unloaded += EditAlbumView_Unloaded;

        }

        protected override void OnOpened()
        {
            base.OnOpened();
            Name.Focus();
            Name.SelectAll();
        }

        void EditAlbumView_Loaded(object sender, RoutedEventArgs e)
        {
            _model = (EditAlbumViewModel)this.DataContext;
            _model.EditCompleted += _model_EditCompleted;
        }

        void EditAlbumView_Unloaded(object sender, RoutedEventArgs e)
        {
            _model.EditCompleted -= _model_EditCompleted;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        void _model_EditCompleted(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }

        private void EditAlbumView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SaveButton.AutomationPeerInvoke();
            }
            else if (e.Key == Key.Escape)
            {
                CancelButton.AutomationPeerInvoke();
            }
        }

    }
}

