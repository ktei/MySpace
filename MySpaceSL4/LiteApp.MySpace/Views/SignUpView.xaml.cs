using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using LiteApp.MySpace.ViewModels;

namespace LiteApp.MySpace.Views
{
    public partial class SignUpView : ChildWindow
    {
        SignUpViewModel _model;

        public SignUpView()
        {
            InitializeComponent();
            this.Loaded += RegistrationView_Loaded;
            this.Unloaded += SignUpView_Unloaded;
        }

        void RegistrationView_Loaded(object sender, RoutedEventArgs e)
        {
            _model = (SignUpViewModel)this.DataContext;
            _model.PasswordAccessor = () => Password.Password;
            _model.PasswordConfirmationAccessor = () => PasswordConfirmation.Password;
            _model.SignUpCompleted += _model_SignUpCompleted;
        }

        void SignUpView_Unloaded(object sender, RoutedEventArgs e)
        {
            _model.SignUpCompleted -= _model_SignUpCompleted;
        }

        void _model_SignUpCompleted(object sender, EventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void SignUpView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(OKButton);
                IInvokeProvider invokeProv = (IInvokeProvider)peer.GetPattern(PatternInterface.Invoke);
                invokeProv.Invoke();
            }
            else if (e.Key == Key.Escape)
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(CancelButton);
                IInvokeProvider invokeProv = (IInvokeProvider)peer.GetPattern(PatternInterface.Invoke);
                invokeProv.Invoke();
            }
        }
    }
}

