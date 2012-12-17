using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteApp.MySpace.Web.Shared
{
    public interface IPhotoUploadTicketPool
    {
        bool VerifyTicket(string requestToken, string ticket);
        string GenerateTicket(string requestToken);
    }
}