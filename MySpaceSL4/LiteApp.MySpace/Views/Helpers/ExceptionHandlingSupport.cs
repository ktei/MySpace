using System;
using System.ServiceModel;
using Caliburn.Micro;
using LiteApp.MySpace.Services.Photo;
using LiteApp.MySpace.ViewModels;

namespace LiteApp.MySpace.Views.Helpers
{
    public static class ExceptionHandlingSupport
    {
        public static void Handle(this Exception ex)
        {
            var model = new MessageBoxViewModel() { MessageLevel = MessageLevel.Error, Buttons = MessageBoxButtons.OK, Header = "Operation failed" };
            if (ex is FaultException<ServerFault>)
            {
                FaultException<ServerFault> faultEx = (FaultException<ServerFault>)ex;
                if (faultEx.Detail.FaultCode == ServerFaultCode.Generic)
                {
                    model.Message = "We apologize that an error occurred on our server. Please try again later.";
                }
                else if (faultEx.Detail.FaultCode == ServerFaultCode.Unauthenticated)
                {
                    model.Message = "Our server detected that you are not authorized to do this.";
                }
            }
            else
            {
                model.Message = "We apologize for the inconvenience. An unknown error occurred. Please try again later.";
            }

            Execute.OnUIThread(() => IoC.Get<IWindowManager>().ShowDialog(model));
        }
    }
}
