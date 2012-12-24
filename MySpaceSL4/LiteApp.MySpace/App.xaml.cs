using System.Windows;
using Caliburn.Micro;
using LiteApp.MySpace.Assets.Strings;
using LiteApp.MySpace.Security;
using LiteApp.MySpace.ViewModels;
using System.Windows.Browser;
using LiteApp.MySpace.Services.Security;
using System;

namespace LiteApp.MySpace
{
    public partial class App : Application
    {
        public App()
        {
            this.Startup += this.Application_Startup;
            this.UnhandledException += App_UnhandledException;

            InitializeComponent();
        }

        void App_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            MessageBoxViewModel message = new MessageBoxViewModel();
            message.DisplayName = AppStrings.ApplicationErrorWindowTitle;
            message.Header = ErrorStrings.GenericErrorMessageHeader;
            message.Message = ErrorStrings.GenericErrorMessage;
            message.Buttons = MessageBoxButtons.OK;
            message.MessageLevel = MessageLevel.Error;
            IoC.Get<IWindowManager>().ShowDialog(message);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.Resources.Add("SecurityContext", SecurityContext.Current);
            SecurityContext.Current.LoadUser();
            ProcessActivationTicket();
        }

        void ProcessActivationTicket()
        {
            string activationTicket = string.Empty;
            
            if (HtmlPage.Document.QueryString.TryGetValue("activationTicket", out activationTicket))
            {
                SecurityContext.Current.ActivateUser(activationTicket, result =>
                    {
                        if (!string.IsNullOrEmpty(result.Error))
                        {
                            MessageBoxViewModel message = new MessageBoxViewModel();
                            message.DisplayName = AppStrings.ActivationUserWindowTitle;
                            message.Header = AppStrings.FailedMessageHeader;
                            message.Message = result.Error;
                            message.Buttons = MessageBoxButtons.OK;
                            message.MessageLevel = MessageLevel.Exclamation;
                            Execute.OnUIThread(() => IoC.Get<IWindowManager>().ShowDialog(message));
                        }
                        else
                        {
                            var url = string.Format("http://{0}:{1}", Application.Current.Host.Source.Host, Application.Current.Host.Source.Port);
                            HtmlPage.Window.Navigate(new Uri(url));
                        }
                    });
            }
        }
    }
}