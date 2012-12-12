using System;

namespace LiteApp.MySpace.Web.Helpers
{
    public class MySpaceException : Exception
    {
        public MySpaceException(string message)
            : base(message)
        {
        }
    }
}