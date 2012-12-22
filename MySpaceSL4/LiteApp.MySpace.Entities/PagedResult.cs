using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LiteApp.MySpace.Entities
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
