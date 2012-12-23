using System;
using System.Collections.Generic;
using System.Linq;
using LiteApp.MySpace.Entities;
using LiteApp.MySpace.Framework;
using LiteApp.MySpace.Services.Logging;

namespace LiteApp.MySpace.ViewModels
{
    public class LogsPagedDataSource : IPagedDataSource<LogEntryViewModel>
    {

        public void FetchData(int pageIndex, int pageSize, Action<PagedDataResponse<LogEntryViewModel>> responseCallback)
        {
            LoggingServiceClient svc = new LoggingServiceClient();
            svc.GetPagedLogsCompleted += (sender, e) =>
            {
                PagedDataResponse<LogEntryViewModel> response = new PagedDataResponse<LogEntryViewModel>();
                if (e.Error != null)
                {
                    response.Error = e.Error;
                }
                else
                {
                    response = new PagedDataResponse<LogEntryViewModel>();
                    response.TotalItemCount = e.Result.TotalItemCount;
                    response.Items = new List<LogEntryViewModel>();
                    response.Items.AddRange(e.Result.Entities.Select(x => MapToLogEntryViewModel(x)));
                }
                responseCallback(response);
            };
            svc.GetPagedLogsAsync(pageIndex, pageSize);
        }


        LogEntryViewModel MapToLogEntryViewModel(LogEntry entry)
        {
            return new LogEntryViewModel()
            {
                Id = entry.Id,
                Detail = entry.Detail,
                CreatedOn = entry.CreatedOn,
                Level = entry.Level
            };
        }
    }
}
