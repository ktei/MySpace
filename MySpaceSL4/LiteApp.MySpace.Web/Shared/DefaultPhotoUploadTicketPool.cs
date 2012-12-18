using System;
using System.Collections.Generic;

namespace LiteApp.MySpace.Web.Shared
{
    public class DefaultPhotoUploadTicketPool : IPhotoUploadTicketPool
    {
        Dictionary<string, WeakReference> _ticketCache;

        public DefaultPhotoUploadTicketPool()
        {
            _ticketCache = new Dictionary<string, WeakReference>();
        }

        public bool VerifyTicket(string requestToken, string ticket)
        {
            WeakReference wr = null;
            _ticketCache.TryGetValue(requestToken, out wr);
            if (wr != null && wr.IsAlive)
            {
                return Convert.ToString(wr.Target) == ticket;
            }
            return false;
        }

        public string GenerateTicket(string requestToken)
        {
            string newTicket = Guid.NewGuid().ToString();
            _ticketCache[requestToken] = new WeakReference(newTicket, false);
            return newTicket;
        }

        public void DestroyTicket(string requestToken)
        {
            _ticketCache.Remove(requestToken);
        }
    }
}