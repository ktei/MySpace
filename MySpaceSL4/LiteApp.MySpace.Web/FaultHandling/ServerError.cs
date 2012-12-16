using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace LiteApp.MySpace.Web.FaultHandling
{
    [DataContract]
    public class ServerFault
    {
        [DataMember]
        public ServerFaultCode FaultCode { get; set; }
    }
}