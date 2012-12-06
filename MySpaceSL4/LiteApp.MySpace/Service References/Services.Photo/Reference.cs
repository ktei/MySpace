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
namespace LiteApp.MySpace.Services.Photo {
    using System.Runtime.Serialization;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PagedResultOfAlbumYX32z0hB", Namespace="http://schemas.datacontract.org/2004/07/LiteApp.MySpace.Web.Services")]
    public partial class PagedResultOfAlbumYX32z0hB : object, System.ComponentModel.INotifyPropertyChanged {
        
        private System.Collections.Generic.List<LiteApp.MySpace.Services.Photo.Album> EntitiesField;
        
        private int TotalItemCountField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<LiteApp.MySpace.Services.Photo.Album> Entities {
            get {
                return this.EntitiesField;
            }
            set {
                if ((object.ReferenceEquals(this.EntitiesField, value) != true)) {
                    this.EntitiesField = value;
                    this.RaisePropertyChanged("Entities");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int TotalItemCount {
            get {
                return this.TotalItemCountField;
            }
            set {
                if ((this.TotalItemCountField.Equals(value) != true)) {
                    this.TotalItemCountField = value;
                    this.RaisePropertyChanged("TotalItemCount");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Album", Namespace="http://schemas.datacontract.org/2004/07/LiteApp.MySpace.Web.Entities")]
    public partial class Album : object, System.ComponentModel.INotifyPropertyChanged {
        
        private System.Collections.Generic.List<string> CoverURIsField;
        
        private string CreatedByField;
        
        private string DescriptionField;
        
        private string IdField;
        
        private string NameField;
        
        private int PhotoCountField;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public System.Collections.Generic.List<string> CoverURIs {
            get {
                return this.CoverURIsField;
            }
            set {
                if ((object.ReferenceEquals(this.CoverURIsField, value) != true)) {
                    this.CoverURIsField = value;
                    this.RaisePropertyChanged("CoverURIs");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string CreatedBy {
            get {
                return this.CreatedByField;
            }
            set {
                if ((object.ReferenceEquals(this.CreatedByField, value) != true)) {
                    this.CreatedByField = value;
                    this.RaisePropertyChanged("CreatedBy");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Description {
            get {
                return this.DescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.DescriptionField, value) != true)) {
                    this.DescriptionField = value;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Id {
            get {
                return this.IdField;
            }
            set {
                if ((object.ReferenceEquals(this.IdField, value) != true)) {
                    this.IdField = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Name {
            get {
                return this.NameField;
            }
            set {
                if ((object.ReferenceEquals(this.NameField, value) != true)) {
                    this.NameField = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PhotoCount {
            get {
                return this.PhotoCountField;
            }
            set {
                if ((this.PhotoCountField.Equals(value) != true)) {
                    this.PhotoCountField = value;
                    this.RaisePropertyChanged("PhotoCount");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="", ConfigurationName="Services.Photo.PhotoService")]
    public interface PhotoService {
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:PhotoService/GetPagedAlbums", ReplyAction="urn:PhotoService/GetPagedAlbumsResponse")]
        System.IAsyncResult BeginGetPagedAlbums(int pageIndex, int pageSize, System.AsyncCallback callback, object asyncState);
        
        LiteApp.MySpace.Services.Photo.PagedResultOfAlbumYX32z0hB EndGetPagedAlbums(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:PhotoService/SaveAlbum", ReplyAction="urn:PhotoService/SaveAlbumResponse")]
        System.IAsyncResult BeginSaveAlbum(LiteApp.MySpace.Services.Photo.Album album, System.AsyncCallback callback, object asyncState);
        
        void EndSaveAlbum(System.IAsyncResult result);
        
        [System.ServiceModel.OperationContractAttribute(AsyncPattern=true, Action="urn:PhotoService/DeleteAlbum", ReplyAction="urn:PhotoService/DeleteAlbumResponse")]
        System.IAsyncResult BeginDeleteAlbum(string albumId, System.AsyncCallback callback, object asyncState);
        
        void EndDeleteAlbum(System.IAsyncResult result);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface PhotoServiceChannel : LiteApp.MySpace.Services.Photo.PhotoService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GetPagedAlbumsCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        public GetPagedAlbumsCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        public LiteApp.MySpace.Services.Photo.PagedResultOfAlbumYX32z0hB Result {
            get {
                base.RaiseExceptionIfNecessary();
                return ((LiteApp.MySpace.Services.Photo.PagedResultOfAlbumYX32z0hB)(this.results[0]));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PhotoServiceClient : System.ServiceModel.ClientBase<LiteApp.MySpace.Services.Photo.PhotoService>, LiteApp.MySpace.Services.Photo.PhotoService {
        
        private BeginOperationDelegate onBeginGetPagedAlbumsDelegate;
        
        private EndOperationDelegate onEndGetPagedAlbumsDelegate;
        
        private System.Threading.SendOrPostCallback onGetPagedAlbumsCompletedDelegate;
        
        private BeginOperationDelegate onBeginSaveAlbumDelegate;
        
        private EndOperationDelegate onEndSaveAlbumDelegate;
        
        private System.Threading.SendOrPostCallback onSaveAlbumCompletedDelegate;
        
        private BeginOperationDelegate onBeginDeleteAlbumDelegate;
        
        private EndOperationDelegate onEndDeleteAlbumDelegate;
        
        private System.Threading.SendOrPostCallback onDeleteAlbumCompletedDelegate;
        
        private BeginOperationDelegate onBeginOpenDelegate;
        
        private EndOperationDelegate onEndOpenDelegate;
        
        private System.Threading.SendOrPostCallback onOpenCompletedDelegate;
        
        private BeginOperationDelegate onBeginCloseDelegate;
        
        private EndOperationDelegate onEndCloseDelegate;
        
        private System.Threading.SendOrPostCallback onCloseCompletedDelegate;
        
        public PhotoServiceClient() {
        }
        
        public PhotoServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public PhotoServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PhotoServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public PhotoServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
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
        
        public event System.EventHandler<GetPagedAlbumsCompletedEventArgs> GetPagedAlbumsCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> SaveAlbumCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> DeleteAlbumCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> OpenCompleted;
        
        public event System.EventHandler<System.ComponentModel.AsyncCompletedEventArgs> CloseCompleted;
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult LiteApp.MySpace.Services.Photo.PhotoService.BeginGetPagedAlbums(int pageIndex, int pageSize, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginGetPagedAlbums(pageIndex, pageSize, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        LiteApp.MySpace.Services.Photo.PagedResultOfAlbumYX32z0hB LiteApp.MySpace.Services.Photo.PhotoService.EndGetPagedAlbums(System.IAsyncResult result) {
            return base.Channel.EndGetPagedAlbums(result);
        }
        
        private System.IAsyncResult OnBeginGetPagedAlbums(object[] inValues, System.AsyncCallback callback, object asyncState) {
            int pageIndex = ((int)(inValues[0]));
            int pageSize = ((int)(inValues[1]));
            return ((LiteApp.MySpace.Services.Photo.PhotoService)(this)).BeginGetPagedAlbums(pageIndex, pageSize, callback, asyncState);
        }
        
        private object[] OnEndGetPagedAlbums(System.IAsyncResult result) {
            LiteApp.MySpace.Services.Photo.PagedResultOfAlbumYX32z0hB retVal = ((LiteApp.MySpace.Services.Photo.PhotoService)(this)).EndGetPagedAlbums(result);
            return new object[] {
                    retVal};
        }
        
        private void OnGetPagedAlbumsCompleted(object state) {
            if ((this.GetPagedAlbumsCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.GetPagedAlbumsCompleted(this, new GetPagedAlbumsCompletedEventArgs(e.Results, e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void GetPagedAlbumsAsync(int pageIndex, int pageSize) {
            this.GetPagedAlbumsAsync(pageIndex, pageSize, null);
        }
        
        public void GetPagedAlbumsAsync(int pageIndex, int pageSize, object userState) {
            if ((this.onBeginGetPagedAlbumsDelegate == null)) {
                this.onBeginGetPagedAlbumsDelegate = new BeginOperationDelegate(this.OnBeginGetPagedAlbums);
            }
            if ((this.onEndGetPagedAlbumsDelegate == null)) {
                this.onEndGetPagedAlbumsDelegate = new EndOperationDelegate(this.OnEndGetPagedAlbums);
            }
            if ((this.onGetPagedAlbumsCompletedDelegate == null)) {
                this.onGetPagedAlbumsCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnGetPagedAlbumsCompleted);
            }
            base.InvokeAsync(this.onBeginGetPagedAlbumsDelegate, new object[] {
                        pageIndex,
                        pageSize}, this.onEndGetPagedAlbumsDelegate, this.onGetPagedAlbumsCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult LiteApp.MySpace.Services.Photo.PhotoService.BeginSaveAlbum(LiteApp.MySpace.Services.Photo.Album album, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginSaveAlbum(album, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void LiteApp.MySpace.Services.Photo.PhotoService.EndSaveAlbum(System.IAsyncResult result) {
            base.Channel.EndSaveAlbum(result);
        }
        
        private System.IAsyncResult OnBeginSaveAlbum(object[] inValues, System.AsyncCallback callback, object asyncState) {
            LiteApp.MySpace.Services.Photo.Album album = ((LiteApp.MySpace.Services.Photo.Album)(inValues[0]));
            return ((LiteApp.MySpace.Services.Photo.PhotoService)(this)).BeginSaveAlbum(album, callback, asyncState);
        }
        
        private object[] OnEndSaveAlbum(System.IAsyncResult result) {
            ((LiteApp.MySpace.Services.Photo.PhotoService)(this)).EndSaveAlbum(result);
            return null;
        }
        
        private void OnSaveAlbumCompleted(object state) {
            if ((this.SaveAlbumCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.SaveAlbumCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void SaveAlbumAsync(LiteApp.MySpace.Services.Photo.Album album) {
            this.SaveAlbumAsync(album, null);
        }
        
        public void SaveAlbumAsync(LiteApp.MySpace.Services.Photo.Album album, object userState) {
            if ((this.onBeginSaveAlbumDelegate == null)) {
                this.onBeginSaveAlbumDelegate = new BeginOperationDelegate(this.OnBeginSaveAlbum);
            }
            if ((this.onEndSaveAlbumDelegate == null)) {
                this.onEndSaveAlbumDelegate = new EndOperationDelegate(this.OnEndSaveAlbum);
            }
            if ((this.onSaveAlbumCompletedDelegate == null)) {
                this.onSaveAlbumCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnSaveAlbumCompleted);
            }
            base.InvokeAsync(this.onBeginSaveAlbumDelegate, new object[] {
                        album}, this.onEndSaveAlbumDelegate, this.onSaveAlbumCompletedDelegate, userState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.IAsyncResult LiteApp.MySpace.Services.Photo.PhotoService.BeginDeleteAlbum(string albumId, System.AsyncCallback callback, object asyncState) {
            return base.Channel.BeginDeleteAlbum(albumId, callback, asyncState);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        void LiteApp.MySpace.Services.Photo.PhotoService.EndDeleteAlbum(System.IAsyncResult result) {
            base.Channel.EndDeleteAlbum(result);
        }
        
        private System.IAsyncResult OnBeginDeleteAlbum(object[] inValues, System.AsyncCallback callback, object asyncState) {
            string albumId = ((string)(inValues[0]));
            return ((LiteApp.MySpace.Services.Photo.PhotoService)(this)).BeginDeleteAlbum(albumId, callback, asyncState);
        }
        
        private object[] OnEndDeleteAlbum(System.IAsyncResult result) {
            ((LiteApp.MySpace.Services.Photo.PhotoService)(this)).EndDeleteAlbum(result);
            return null;
        }
        
        private void OnDeleteAlbumCompleted(object state) {
            if ((this.DeleteAlbumCompleted != null)) {
                InvokeAsyncCompletedEventArgs e = ((InvokeAsyncCompletedEventArgs)(state));
                this.DeleteAlbumCompleted(this, new System.ComponentModel.AsyncCompletedEventArgs(e.Error, e.Cancelled, e.UserState));
            }
        }
        
        public void DeleteAlbumAsync(string albumId) {
            this.DeleteAlbumAsync(albumId, null);
        }
        
        public void DeleteAlbumAsync(string albumId, object userState) {
            if ((this.onBeginDeleteAlbumDelegate == null)) {
                this.onBeginDeleteAlbumDelegate = new BeginOperationDelegate(this.OnBeginDeleteAlbum);
            }
            if ((this.onEndDeleteAlbumDelegate == null)) {
                this.onEndDeleteAlbumDelegate = new EndOperationDelegate(this.OnEndDeleteAlbum);
            }
            if ((this.onDeleteAlbumCompletedDelegate == null)) {
                this.onDeleteAlbumCompletedDelegate = new System.Threading.SendOrPostCallback(this.OnDeleteAlbumCompleted);
            }
            base.InvokeAsync(this.onBeginDeleteAlbumDelegate, new object[] {
                        albumId}, this.onEndDeleteAlbumDelegate, this.onDeleteAlbumCompletedDelegate, userState);
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
        
        protected override LiteApp.MySpace.Services.Photo.PhotoService CreateChannel() {
            return new PhotoServiceClientChannel(this);
        }
        
        private class PhotoServiceClientChannel : ChannelBase<LiteApp.MySpace.Services.Photo.PhotoService>, LiteApp.MySpace.Services.Photo.PhotoService {
            
            public PhotoServiceClientChannel(System.ServiceModel.ClientBase<LiteApp.MySpace.Services.Photo.PhotoService> client) : 
                    base(client) {
            }
            
            public System.IAsyncResult BeginGetPagedAlbums(int pageIndex, int pageSize, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[2];
                _args[0] = pageIndex;
                _args[1] = pageSize;
                System.IAsyncResult _result = base.BeginInvoke("GetPagedAlbums", _args, callback, asyncState);
                return _result;
            }
            
            public LiteApp.MySpace.Services.Photo.PagedResultOfAlbumYX32z0hB EndGetPagedAlbums(System.IAsyncResult result) {
                object[] _args = new object[0];
                LiteApp.MySpace.Services.Photo.PagedResultOfAlbumYX32z0hB _result = ((LiteApp.MySpace.Services.Photo.PagedResultOfAlbumYX32z0hB)(base.EndInvoke("GetPagedAlbums", _args, result)));
                return _result;
            }
            
            public System.IAsyncResult BeginSaveAlbum(LiteApp.MySpace.Services.Photo.Album album, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = album;
                System.IAsyncResult _result = base.BeginInvoke("SaveAlbum", _args, callback, asyncState);
                return _result;
            }
            
            public void EndSaveAlbum(System.IAsyncResult result) {
                object[] _args = new object[0];
                base.EndInvoke("SaveAlbum", _args, result);
            }
            
            public System.IAsyncResult BeginDeleteAlbum(string albumId, System.AsyncCallback callback, object asyncState) {
                object[] _args = new object[1];
                _args[0] = albumId;
                System.IAsyncResult _result = base.BeginInvoke("DeleteAlbum", _args, callback, asyncState);
                return _result;
            }
            
            public void EndDeleteAlbum(System.IAsyncResult result) {
                object[] _args = new object[0];
                base.EndInvoke("DeleteAlbum", _args, result);
            }
        }
    }
}
