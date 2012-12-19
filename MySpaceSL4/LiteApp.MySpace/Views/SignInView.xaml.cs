using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LiteApp.MySpace.ViewModels;
using LiteApp.MySpace.Views.Helpers;

namespace LiteApp.MySpace.Views
{
    public partial class SignInView : ChildWindow
    {
        SignInViewModel _model;

        public SignInView()
        {
            InitializeComponent();
            this.Loaded += SignInView_Loaded;
            this.Unloaded += SignInView_Unloaded;
        }

        protected override void OnOpened()
        {
            base.OnOpened();
            UserName.Focus();
        }

        void SignInView_Loaded(object sender, RoutedEventArgs e)
        {
            _model = (SignInViewModel)this.DataContext;
            _model.PasswordAccessor = () => Password.Password;
            _model.SignInSucceeded += _model_SignInSucceeded;
        }

        void SignInView_Unloaded(object sender, RoutedEventArgs e)
        {
            _model.SignInSucceeded -= _model_SignInSucceeded;
        }

        void _model_SignInSucceeded(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SignUpLinkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SignInView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OKButton.AutomationPeerInvoke();
            }
            else if (e.Key == Key.Escape)
            {
                CancelButton.AutomationPeerInvoke();
            }
        }
    }
}

