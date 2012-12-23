using System.Windows;
using Caliburn.Micro;
using LiteApp.MySpace.Assets.Strings;
using LiteApp.MySpace.Security;
using LiteApp.MySpace.ViewModels;

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
            message.Header = ErrorStrings.GenericErrorMessageHeader;
            message.Message = ErrorStrings.GenericErrorMessage;
            message.Buttons = MessageBoxButtons.OK;
            message.MessageLevel = MessageLevel.Error;
            IoC.Get<IWindowManager>().ShowDialog(message);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.Resources.Add("SecurityContext", SecurityContext.Current);
        }
    }
}