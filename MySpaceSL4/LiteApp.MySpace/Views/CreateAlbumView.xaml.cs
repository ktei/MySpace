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

namespace LiteApp.MySpace.Views
{
    public partial class CreateAlbumView : ChildWindow
    {
        public CreateAlbumView()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void CreateAlbumView_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}

