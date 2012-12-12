using System;

namespace LiteApp.MySpace.Web.ErrorHandling
{
    public class MySpaceException : Exception
    {
        public MySpaceException(string message)
            : base(message)
        {
        }
    }
}