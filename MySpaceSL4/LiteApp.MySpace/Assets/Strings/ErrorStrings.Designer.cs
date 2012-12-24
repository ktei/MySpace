﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LiteApp.MySpace.Assets.Strings {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ErrorStrings {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ErrorStrings() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("LiteApp.MySpace.Assets.Strings.ErrorStrings", typeof(ErrorStrings).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Activation failed because the activation ticket is invalid..
        /// </summary>
        public static string ActivationFailed_InvalidTicket {
            get {
                return ResourceManager.GetString("ActivationFailed_InvalidTicket", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Activation failed because server experienced an error. Please try again later..
        /// </summary>
        public static string ActivationFailed_ServerError {
            get {
                return ResourceManager.GetString("ActivationFailed_ServerError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to It seems some error just occurred. We apologize for the inconvenience. Please try again later..
        /// </summary>
        public static string GenericErrorMessage {
            get {
                return ResourceManager.GetString("GenericErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Oops!.
        /// </summary>
        public static string GenericErrorMessageHeader {
            get {
                return ResourceManager.GetString("GenericErrorMessageHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to We apologize that an error occurred on our server. Please try again later..
        /// </summary>
        public static string GenericServerErrorMessage {
            get {
                return ResourceManager.GetString("GenericServerErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your account has not been activated..
        /// </summary>
        public static string InactiveAccountMessage {
            get {
                return ResourceManager.GetString("InactiveAccountMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Provided credentials are invalid. Please check your user name and password..
        /// </summary>
        public static string InvalidCredentialsMessage {
            get {
                return ResourceManager.GetString("InvalidCredentialsMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The e-mail address provided is invalid. Please check the value and try again..
        /// </summary>
        public static string InvalidEmail {
            get {
                return ResourceManager.GetString("InvalidEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Our server detected that you are not authorized to do this..
        /// </summary>
        public static string NotAuthorizedServerErrorMessage {
            get {
                return ResourceManager.GetString("NotAuthorizedServerErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Passwords do not match..
        /// </summary>
        public static string PasswordsDoNotMatch {
            get {
                return ResourceManager.GetString("PasswordsDoNotMatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An account with the same email already exists. Please sign up with a different email..
        /// </summary>
        public static string SignUpFailed_DuplicateEmail {
            get {
                return ResourceManager.GetString("SignUpFailed_DuplicateEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An account with the same user name already exists. Please sign up with a different user name..
        /// </summary>
        public static string SignUpFailed_DuplicateUserName {
            get {
                return ResourceManager.GetString("SignUpFailed_DuplicateUserName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The password is invalid. Please sign up with a valid password..
        /// </summary>
        public static string SignUpFailed_InvalidPassword {
            get {
                return ResourceManager.GetString("SignUpFailed_InvalidPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to We apologize for the inconvenience. An unknown error occurred. Please try again later..
        /// </summary>
        public static string UnknownServerErrorMessage {
            get {
                return ResourceManager.GetString("UnknownServerErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to User {0} has already been activated..
        /// </summary>
        public static string UserAlreadyActivatedMessage {
            get {
                return ResourceManager.GetString("UserAlreadyActivatedMessage", resourceCulture);
            }
        }
    }
}
