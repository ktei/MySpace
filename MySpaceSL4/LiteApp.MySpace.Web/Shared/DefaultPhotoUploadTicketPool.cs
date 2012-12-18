using System;
using System.Collections.Generic;

namespace LiteApp.MySpace.Web.Shared
{
    public class DefaultPhotoUploadTicketPool : IPhotoUploadTicketPool
    {
        Dictionary<string, string> _ticketCache;

        public DefaultPhotoUploadTicketPool()
        {
            _ticketCache = new Dictionary<string, string>();
        }

        // TODO: dictionary thread safety?
        public bool VerifyTicket(string requestToken, string ticket)
        {
            string target = null;
            _ticketCache.TryGetValue(requestToken, out target);
            if (target != null)
            {
                return target == ticket;
            }
            return false;
        }

        public string GenerateTicket(string requestToken)
        {
            string newTicket = Guid.NewGuid().ToString();
            _ticketCache[requestToken] = newTicket;
            return newTicket;
        }

        public void DestroyTicket(string requestToken)
        {
            _ticketCache.Remove(requestToken);
        }
    }
}