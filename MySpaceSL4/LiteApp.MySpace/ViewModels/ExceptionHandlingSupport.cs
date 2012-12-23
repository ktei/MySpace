using System;
using System.ServiceModel;
using Caliburn.Micro;
using LiteApp.MySpace.Services.Photo;
using LiteApp.MySpace.ViewModels;
using LiteApp.MySpace.Assets.Strings;

namespace LiteApp.MySpace.ViewModels
{
    public static class ExceptionHandlingSupport
    {
        public static void Handle(this Exception ex)
        {
            var model = new MessageBoxViewModel() { MessageLevel = MessageLevel.Error, Buttons = MessageBoxButtons.OK, Header = AppStrings.OperationFailedMessageHeader };
            if (ex is FaultException<ServerFault>)
            {
                FaultException<ServerFault> faultEx = (FaultException<ServerFault>)ex;
                if (faultEx.Detail.FaultCode == ServerFaultCode.Generic)
                {
                    model.Message = ErrorStrings.GenericServerErrorMessage;
                }
                else if (faultEx.Detail.FaultCode == ServerFaultCode.NotAuthroized)
                {
                    model.Message = ErrorStrings.NotAuthorizedServerErrorMessage;
                }
            }
            else
            {
                model.Message = ex.ToString(); //ErrorStrings.UnknownServerErrorMessage;
            }

            Execute.OnUIThread(() => IoC.Get<IWindowManager>().ShowDialog(model));
        }
    }
}
