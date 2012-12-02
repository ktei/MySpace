using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LiteApp.MySpace.Web.Services
{
    public enum SignUpStatus
    {
        // Summary:
        //     The user was successfully created.
        Success = 0,
        //
        // Summary:
        //     The password is not formatted correctly.
        InvalidPassword,
        //
        // Summary:
        //     The e-mail address is not formatted correctly.
        InvalidEmail,
        //
        // Summary:
        //     The user name already exists in the database for the application.
        DuplicateUserName,
        //
        // Summary:
        //     The e-mail address already exists in the database for the application.
        DuplicateEmail,

        ServerError
    }
}