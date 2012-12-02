﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.296
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This code was auto-generated by Microsoft.Silverlight.ServiceReference, version 4.0.50826.0
// 
namespace LiteApp.MySpace.Services.Security {
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SignInStaus", Namespace="http://schemas.datacontract.org/2004/07/LiteApp.MySpace.Web.Services")]
    public enum SignInStaus : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Success = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        WrongCredentials = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ServerError = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SignUpStatus", Namespace="http://schemas.datacontract.org/2004/07/LiteApp.MySpace.Web.Services")]
    public enum SignUpStatus : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Success = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        InvalidPassword = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        InvalidEmail = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DuplicateUserName = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        DuplicateEmail = 4,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ServerError = 5,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="", ConfigurationName="Services.Security.SecurityService")]
    public interface SecurityService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:SecurityService/SignIn", ReplyAction="urn:SecurityService/SignInResponse")]
        System.IAsyncResult BeginSignIn(string userName, string password, System.AsyncCallback callback, object asyncState);
        
        LiteApp.MySpace.Services.Security.SignInStaus EndSignIn(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:SecurityService/SignUp", ReplyAction="urn:SecurityService/SignUpResponse")]
        System.IAsyncResult BeginSignUp(string userName, string password, string email, System.AsyncCallback callback, object asyncState);
        
        LiteApp.MySpace.Services.Security.SignUpStatus EndSignUp(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface SecurityServiceChannel : LiteApp.MySpace.Services.Security.SecurityService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SignInCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public SignInCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public LiteApp.MySpace.Services.Security.SignInStaus Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((LiteApp.MySpace.Services.Security.SignInStaus)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SignUpCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public SignUpCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public LiteApp.MySpace.Services.Security.SignUpStatus Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((LiteApp.MySpace.Services.Security.SignUpStatus)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class SecurityServiceClient : System.ServiceModel.ClientBase<LiteApp.MySpace.Services.Security.SecurityService>, LiteApp.MySpace.Services.Security.SecurityService {
        
        private BeginOperationDelegate onBeginSignInDelegate;
        
        private EndOperationDelegate onEndSignInDelegate;
        
        private System.Threading.SendOrPostCallback onSignInCompletedDelegate;
        
        private BeginOperationDelegate onBeginSignUpDelegate;
        
        private EndOperationDelegate onEndSignUpDelegate;
        
        private System.Threading.SendOrPostCallback onSignUpCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public SecurityServiceClient() {
        }
        
        public SecurityServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public SecurityServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SecurityServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public SecurityServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public System.Net.CookieContainer CookieContainer {
            get {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    return httpCookieContainerManager.CookieContainer;
                }
                else {
                    return null;
                }
            }
            set {
                System.ServiceModel.Channels.IHttpCookieContainerManager httpCookieContainerManager = this.InnerChannel.GetProperty<System.ServiceModel.Channels.IHttpCookieContainerManager>();
                if ((httpCookieContainerManager != null)) {
                    httpCookieContainerManager.CookieContainer = value;
                }
                else {
                    throw new System.InvalidOperationException("Unable to set the CookieContainer. Please make sure the binding contains an HttpC" +
                            "ookieContainerBindingElement.");
                }
            }
        }
        
        public event System.EventHandler<SignInCompletedEventArgs> SignInCompleted;
        
        public event System.EventHandler<SignUpCompletedEventArgs> SignUpCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult LiteApp.MySpace.Services.Security.SecurityService.BeginSignIn(string userName, string password, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginSignIn(userName, password, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        LiteApp.MySpace.Services.Security.SignInStaus LiteApp.MySpace.Services.Security.SecurityService.EndSignIn(System.IAsyncResult result) {
            return base.Channel.EndSignIn(result);
        }
        
        private System.IAsyncResult OnBeginSignIn(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string userName = ((string)(inValues[0]));
            string password = ((string)(inValues[1]));
            return ((LiteApp.MySpace.Services.Security.SecurityService)(this)).BeginSignIn(userName, password, callback, asyncState);
        }
        
        private object[] OnEndSignIn(System.IAsyncResult result) {
            LiteApp.MySpace.Services.Security.SignInStaus retVal = ((LiteApp.MySpace.Services.Security.SecurityService)(this)).EndSignIn(result);
            return new object[] {
                    retVal};
        }
        
        private void OnSignInCompleted(object state) {
            if ((this.SignInCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.SignInCompleted(this, new SignInCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void SignInAsync(string userName, string password) {
            this.SignInAsync(userName, password, null);
        }
        
        public void SignInAsync(string userName, string password, object userState) {
            if ((this.onBeginSignInDelegate == null)) {
                this.onBeginSignInDelegate = new BeginOperationDelegate(this.OnBeginSignIn);
            }
            if ((this.onEndSignInDelegate == null)) {
                this.onEndSignInDelegate = new EndOperationDelegate(this.OnEndSignIn);
            }
            if ((this.onSignInCompletedDelegate == null)) {
                this.onSignInCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnSignInCompleted);
            }
            base.InvokeAsync(this.onBeginSignInDelegate, new object[] {
                        userName,
                        password}, this.onEndSignInDelegate, this.onSignInCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult LiteApp.MySpace.Services.Security.SecurityService.BeginSignUp(string userName, string password, string email, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginSignUp(userName, password, email, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        LiteApp.MySpace.Services.Security.SignUpStatus LiteApp.MySpace.Services.Security.SecurityService.EndSignUp(System.IAsyncResult result) {
            return base.Channel.EndSignUp(result);
        }
        
        private System.IAsyncResult OnBeginSignUp(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string userName = ((string)(inValues[0]));
            string password = ((string)(inValues[1]));
            string email = ((string)(inValues[2]));
            return ((LiteApp.MySpace.Services.Security.SecurityService)(this)).BeginSignUp(userName, password, email, callback, asyncState);
        }
        
        private object[] OnEndSignUp(System.IAsyncResult result) {
            LiteApp.MySpace.Services.Security.SignUpStatus retVal = ((LiteApp.MySpace.Services.Security.SecurityService)(this)).EndSignUp(result);
            return new object[] {
                    retVal};
        }
        
        private void OnSignUpCompleted(object state) {
            if ((this.SignUpCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.SignUpCompleted(this, new SignUpCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void SignUpAsync(string userName, string password, string email) {
            this.SignUpAsync(userName, password, email, null);
        }
        
        public void SignUpAsync(string userName, string password, string email, object userState) {
            if ((this.onBeginSignUpDelegate == null)) {
                this.onBeginSignUpDelegate = new BeginOperationDelegate(this.OnBeginSignUp);
            }
            if ((this.onEndSignUpDelegate == null)) {
                this.onEndSignUpDelegate = new EndOperationDelegate(this.OnEndSignUp);
            }
            if ((this.onSignUpCompletedDelegate == null)) {
                this.onSignUpCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnSignUpCompleted);
            }
            base.InvokeAsync(this.onBeginSignUpDelegate, new object[] {
                        userName,
                        password,
                        email}, this.onEndSignUpDelegate, this.onSignUpCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginOpen(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginOpen(callback, asyncState);
        }
        
        private object[] OnEndOpen(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndOpen(result);
            return null;
        }
        
        private void OnOpenCompleted(object state) {
            if ((this.OpenCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.OpenCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void OpenAsync() {
            this.OpenAsync(null);
        }
        
        public void OpenAsync(object userState) {
            if ((this.onBeginOpenDelegate == null)) {
                this.onBeginOpenDelegate = new BeginOperationDelegate(this.OnBeginOpen);
            }
            if ((this.onEndOpenDelegate == null)) {
                this.onEndOpenDelegate = new EndOperationDelegate(this.OnEndOpen);
            }
            if ((this.onOpenCompletedDelegate == null)) {
                this.onOpenCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnOpenCompleted);
            }
            base.InvokeAsync(this.onBeginOpenDelegate, null, this.onEndOpenDelegate, this.onOpenCompletedDelegate, userState);
        }
        
        private System.IAsyncResult OnBeginClose(object[] inValues, System.AsyncCallback callback, object asyncState) {
            return ((System.ServiceModel.ICommunicationObject)(this)).BeginClose(callback, asyncState);
        }
        
        private object[] OnEndClose(System.IAsyncResult result) {
            ((System.ServiceModel.ICommunicationObject)(this)).EndClose(result);
            return null;
        }
        
        private void OnCloseCompleted(object state) {
            if ((this.CloseCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.CloseCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void CloseAsync() {
            this.CloseAsync(null);
        }
        
        public void CloseAsync(object userState) {
            if ((this.onBeginCloseDelegate == null)) {
                this.onBeginCloseDelegate = new BeginOperationDelegate(this.OnBeginClose);
            }
            if ((this.onEndCloseDelegate == null)) {
                this.onEndCloseDelegate = new EndOperationDelegate(this.OnEndClose);
            }
            if ((this.onCloseCompletedDelegate == null)) {
                this.onCloseCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnCloseCompleted);
            }
            base.InvokeAsync(this.onBeginCloseDelegate, null, this.onEndCloseDelegate, this.onCloseCompletedDelegate, userState);
        }
        
        protected override LiteApp.MySpace.Services.Security.SecurityService CreateChannel() {
            return new SecurityServiceClientChannel(this);
        }
        
        private class SecurityServiceClientChannel : ChannelBase<LiteApp.MySpace.Services.Security.SecurityService>, LiteApp.MySpace.Services.Security.SecurityService {
            
            public SecurityServiceClientChannel(System.ServiceModel.ClientBase<LiteApp.MySpace.Services.Security.SecurityService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginSignIn(string userName, string password, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = userName;
                _args[1] = password;
                System.IAsyncResult _result = base.BeginInvoke("SignIn", _args, callback, asyncState);
                return _result;
            }
            
            public LiteApp.MySpace.Services.Security.SignInStaus EndSignIn(System.IAsyncResult result) {
                object[] _args = new object[0];
                LiteApp.MySpace.Services.Security.SignInStaus _result = ((LiteApp.MySpace.Services.Security.SignInStaus)(base.EndInvoke("SignIn", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginSignUp(string userName, string password, string email, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[3];
                _args[0] = userName;
                _args[1] = password;
                _args[2] = email;
                System.IAsyncResult _result = base.BeginInvoke("SignUp", _args, callback, asyncState);
                return _result;
            }
            
            public LiteApp.MySpace.Services.Security.SignUpStatus EndSignUp(System.IAsyncResult result) {
                object[] _args = new object[0];
                LiteApp.MySpace.Services.Security.SignUpStatus _result = ((LiteApp.MySpace.Services.Security.SignUpStatus)(base.EndInvoke("SignUp", _args, result)));
                return _result;
            }
        }
    }
}
