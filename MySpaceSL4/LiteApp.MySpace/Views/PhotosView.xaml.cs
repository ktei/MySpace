﻿using System.Windows;
using System.Windows.Controls;
using LiteApp.MySpace.ViewModels;

namespace LiteApp.MySpace.Views
{
    public partial class PhotosView : Page
    {
        public PhotosView()
        {
            InitializeComponent();
        }

        private void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            ((CoverViewModel)(sender as Image).DataContext).IsLoading = false;
        }
    }
}
