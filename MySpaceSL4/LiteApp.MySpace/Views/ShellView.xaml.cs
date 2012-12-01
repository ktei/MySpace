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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Caliburn.Micro;
using LiteApp.MySpace.ViewModels;

namespace LiteApp.MySpace.Views
{
    public partial class ShellView : UserControl
    {
        ShellViewModel _model;

        public ShellView()
        {
            InitializeComponent();
            this.Loaded += ShellView_Loaded;
            this.LayoutUpdated += ShellView_LayoutUpdated;
        }

        void ShellView_Loaded(object sender, RoutedEventArgs e)
        {
            _model = (ShellViewModel)this.DataContext;
            _model.PageActivatged += _model_PageActivatged;
        }

        void _model_PageActivatged(object sender, PageActivatedEventArgs e)
        {
            UpdateLinkStates();
        }

        void ShellView_LayoutUpdated(object sender, EventArgs e)
        {
            UpdateLinkStates();
        }

        void UpdateLinkStates()
        {
            var activePageName = _model.ActiveItem.Name;
            foreach (UIElement child in LinksStackPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null)
                {
                    if (hb.Tag as string == activePageName)
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
        }
    }
}