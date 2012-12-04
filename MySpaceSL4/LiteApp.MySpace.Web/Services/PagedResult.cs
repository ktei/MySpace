using System.Collections.Generic;
using LiteApp.MySpace.Web.Entities;
using System.Runtime.Serialization;

namespace LiteApp.MySpace.Web.Services
{
    [DataContract]
    public class PagedResult<T>
    {
        [DataMember]
        public List<T> Entities { get; set; }

        [DataMember]
        public int TotalItemCount { get; set; }
    }
}