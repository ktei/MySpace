using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteApp.MySpace.Web.Services
{
    public enum SignInStaus
    {
        Success = 0,
        WrongCredentials,
        Inactive,
        ServerError
    }
}