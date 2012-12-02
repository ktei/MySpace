using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using LiteApp.MySpace.ViewModels;

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


        void SignInView_Loaded(object sender, RoutedEventArgs e)
        {
            _model = (SignInViewModel)this.DataContext;
            _model.PasswordAccessor = () => Password.Password;
            _model.SignInCompleted += _model_SignInCompleted;
        }

        void SignInView_Unloaded(object sender, RoutedEventArgs e)
        {
            _model.SignInCompleted -= _model_SignInCompleted;
        }

        void _model_SignInCompleted(object sender, EventArgs e)
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

