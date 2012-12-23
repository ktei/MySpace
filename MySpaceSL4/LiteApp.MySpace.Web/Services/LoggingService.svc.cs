using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading;
using System.Web;
using LiteApp.MySpace.Entities;
using LiteApp.MySpace.Web.DataAccess;
using LiteApp.MySpace.Web.ErrorHandling;
using Ninject;
using Ninject.Web;

namespace LiteApp.MySpace.Web.Services
{
    [ServiceContract(Namespace = "")]
    [SilverlightFaultBehavior]
    [ErrorHandlingBehavior(typeof(GenericErrorHandler))]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class LoggingService : WebServiceBase
    {
        public LoggingService()
        {
            Thread.CurrentPrincipal = HttpContext.Current.User;
        }

        [Inject]
        public ILogRepository LogRepository { get; set; }

        [OperationContract]
        [FaultContract(typeof(ServerFault))]
        public PagedResult<LogEntry> GetPagedLogs(int pageIndex, int pageSize)
        {
            PagedResult<LogEntry> result = null;
            ServiceSupport.AuthorizeAndExecute(() =>
                {
                    result = new PagedResult<LogEntry>();
                    result.Entities = LogRepository.GetPagedLogs(pageIndex, pageSize).ToList();
                    result.TotalItemCount = LogRepository.GetTotalLogCount();
                }, "ktei");
            return result;
        }
    }
}
