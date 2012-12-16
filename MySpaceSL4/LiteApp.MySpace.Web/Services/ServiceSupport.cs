using System;
using System.Security;
using System.Security.Permissions;
using System.ServiceModel;
using LiteApp.MySpace.Web.FaultHandling;

namespace LiteApp.MySpace.Web.Services
{
    public static class ServiceSupport
    {
        public static void Execute(Action action)
        {
            try
            {
                action();
            }
            catch (FaultException<ServerFault>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FaultException<ServerFault>(new ServerFault() { FaultCode = ServerFaultCode.Generic },
                    new FaultReason(ex.Message));
            }
        }

        public static void AuthorizeAndExecute(Action action, string users = null, string roles = null)
        {
            try
            {
                PrincipalPermission permission = new PrincipalPermission(users, roles, true);
                permission.Demand();
                action();
            }
            catch (SecurityException ex)
            {
                throw new FaultException<ServerFault>(new ServerFault() { FaultCode = ServerFaultCode.NotAuthroized },
                    new FaultReason(ex.Message));
            }
            catch (FaultException<ServerFault>)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new FaultException<ServerFault>(new ServerFault() { FaultCode = ServerFaultCode.Generic },
                    new FaultReason(ex.Message));
            }
        }
    }
}