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

namespace LiteApp.MySpace.Views.Photos
{
    public partial class Master : UserControl
    {
        public Master()
        {
            InitializeComponent();
        }

        private void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            ((CoverViewModel)(sender as Image).DataContext).IsLoading = false;
        }
    }
}
