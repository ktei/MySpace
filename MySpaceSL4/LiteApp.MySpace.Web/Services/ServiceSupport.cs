using System;
using System.Security;
using System.Security.Permissions;
using System.ServiceModel;
using LiteApp.MySpace.Web.ErrorHandling;
using System.Web;

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

        public static bool IsSuperAdminLoggedIn(this HttpContext context)
        {
            return context.IsUserLoggedIn("ktei");
        }

        public static bool IsUserLoggedIn(this HttpContext context, string userName)
        {
            return context.User.Identity.IsAuthenticated && context.User.Identity.Name == userName;
        }
    }
}