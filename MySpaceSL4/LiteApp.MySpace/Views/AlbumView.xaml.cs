using System.Windows;
using System.Windows.Controls;
using LiteApp.MySpace.ViewModels;

namespace LiteApp.MySpace.Views
{
    public partial class AlbumView : UserControl
    {
        public AlbumView()
        {
            InitializeComponent();
        }

        private void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            ((PhotoViewModel)(sender as Image).DataContext).IsLoading = false;
        }
    }
}
