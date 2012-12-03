using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LiteApp.MySpace.Views.Extensions;
using LiteApp.MySpace.ViewModels;

namespace LiteApp.MySpace.Views
{
    public partial class CreateAlbumView : ChildWindow
    {
        CreateAlbumViewModel _model;

        public CreateAlbumView()
        {
            InitializeComponent();
            this.Loaded += CreateAlbumView_Loaded;
            this.Unloaded += CreateAlbumView_Unloaded;
        }

        void CreateAlbumView_Loaded(object sender, RoutedEventArgs e)
        {
            _model = (CreateAlbumViewModel)this.DataContext;
            _model.CreateCompleted += _model_CreateCompleted;
        }

        void CreateAlbumView_Unloaded(object sender, RoutedEventArgs e)
        {
            _model.CreateCompleted -= _model_CreateCompleted;
        }

        void _model_CreateCompleted(object sender, System.EventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void CreateAlbumView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CreateButton.AutomationPeerInvoke();
            }
            else if (e.Key == Key.Escape)
            {
                CancelButton.AutomationPeerInvoke();
            }
        }
    }
}

