using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using LiteApp.MySpace.Web.Helpers;
using LiteApp.MySpace.Web.Logging;
using Ninject;
using System.ServiceModel;

namespace LiteApp.MySpace.Web.ErrorHandling
{
    public class GenericErrorHandler : IErrorHandler
    {
        // Provide a fault. The Message fault parameter can be replaced, or set to
        // null to suppress reporting a fault.

        [Inject]
        public ILogger Logger { get; set; }

        public GenericErrorHandler()
        {
            DI.Inject(this);
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
        }

        // HandleError. Log an error, then allow the error to be handled as usual.
        // Return true if the error is considered as already handled

        public bool HandleError(Exception error)
        {
            if (error is FaultException<ServerFault>)
            {
                Logger.Error("{0}{1}Stack trace:{2}", error.ToString(), Environment.NewLine, error.StackTrace.ToString());
            }
            else
            {
                Logger.Error(error.ToString());
            }
            return true;
        }
    }
}