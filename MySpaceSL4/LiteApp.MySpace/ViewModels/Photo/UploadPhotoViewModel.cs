using System;
using System.IO;
using System.Net;
using System.Net.Browser;
using System.Threading;
using System.Windows;
using Caliburn.Micro;
using LiteApp.MySpace.Services.Security;
using LiteApp.MySpace.Views.Helpers;

namespace LiteApp.MySpace.ViewModels
{
    public class UploadPhotoViewModel : PropertyChangedBase
    {
        string _baseUri;
        private Stream _fileStream;
        private long _dataLength;
        private long _dataSent;
        string _fileName;
        bool _cancelRequested;
        bool _canCancel;
        PhotoUploadStatus _status;
        double _progress;

        public event EventHandler UploadStarted;

        public event EventHandler UploadCanceled;

        public event EventHandler<PhotoUploadCompletedEventArgs> UploadCompleted;

        public double Progress
        {
            get { return _progress; }
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    NotifyOfPropertyChange(() => Progress);
                }
            }

        }

        public string FileName
        {
            get { return _fileName; }
            set
            {
                if (_fileName != value)
                {
                    _fileName = value;
                    NotifyOfPropertyChange(() => FileName);
                }
            }
        }

        public PhotoUploadStatus Status
        {
            get { return _status; }
            set
            {
                if (_status != value)
                {
                    _status = value;
                    NotifyOfPropertyChange(() => Status);
                }
            }
        }

        public bool CanCancel
        {
            get { return _canCancel; }
            set
            {
                if (_canCancel != value)
                {
                    _canCancel = value;
                    NotifyOfPropertyChange(() => CanCancel);
                }
            }
        }

        public UploadPhotoViewModel()
        {
            Uri fullUri = Application.Current.Host.Source;
            _baseUri = fullUri.AbsoluteUri.Substring(0, fullUri.AbsoluteUri.IndexOf("/ClientBin"));
        }

        public void StartUpload(FileInfo file, string albumId)
        {
            Status = PhotoUploadStatus.Uploading;
            // Generate a request token locally and use it
            // to ask server for a photo upload ticket
            // Once ticket received, send both request token and ticket
            // to server for upload authentication
            string requestToken = Guid.NewGuid().ToString();
            SecurityServiceClient svc = new SecurityServiceClient();
            svc.RequestPhotoUploadTicketCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        Status = PhotoUploadStatus.Error;
                    }
                    else if (!string.IsNullOrEmpty(e.Result))
                    {
                        _fileStream = file.OpenRead();
                        _dataLength = _fileStream.Length;
                        UriBuilder httpHandlerUrlBuilder = new UriBuilder(string.Format("{0}/Handlers/PhotoReceiver.ashx", _baseUri));
                        httpHandlerUrlBuilder.Query = string.Format("{0}extension={1}&albumId={2}&requestToken={3}&ticket={4}",
                            string.IsNullOrEmpty(httpHandlerUrlBuilder.Query) ? "" : httpHandlerUrlBuilder.Query.Remove(0, 1) + "&",
                            Path.GetExtension(file.Name),
                            albumId,
                            requestToken,
                            e.Result);

                        HttpWebRequest webRequest = (HttpWebRequest)WebRequestCreator.ClientHttp.Create(httpHandlerUrlBuilder.Uri);
                        webRequest.AllowWriteStreamBuffering = false; // Enable ongoing progress reporting
                        webRequest.ContentType = "multipart/form-data";
                        webRequest.ContentLength = _dataLength;
                        webRequest.Method = "POST";
                        webRequest.BeginGetRequestStream(new AsyncCallback(WriteToStreamCallback), webRequest);
                        FileName = file.Name;
                        CanCancel = true;
                        if (UploadStarted != null)
                            UploadStarted(this, EventArgs.Empty);
                    }
                    else
                    {
                        // TODO: Inform users they don't are not authenticated to do this
                    }
                };
            svc.RequestPhotoUploadTicketAsync(requestToken);
        }

        public void CancelUpload()
        {
            CanCancel = false;
            Status = PhotoUploadStatus.Canceled;
            _cancelRequested = true;
        }

        private void WriteToStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
            try
            {
                using (Stream requestStream = webRequest.EndGetRequestStream(asynchronousResult))
                {

                    byte[] buffer = new Byte[4096];
                    int bytesRead = 0;

                    //Set the start position
                    _fileStream.Position = 0;

                    //Keep reading and flush to server
                    using (_fileStream)
                    {
                        while ((bytesRead = _fileStream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            if (_cancelRequested)
                            {
                                break;
                            }

                            requestStream.Write(buffer, 0, bytesRead);
                            requestStream.Flush();
                            _dataSent += bytesRead;
                            Progress = (double)_dataSent / (double)_dataLength;
                        }
                    }

                    if (_cancelRequested)
                    {
                        if (UploadCanceled != null)
                            UploadCanceled(this, EventArgs.Empty);
                    }
                    else
                    {
                        CanCancel = false;
                        Status = PhotoUploadStatus.Completing;
                        //Get the response from the HttpHandler
                        requestStream.Close();
                        webRequest.BeginGetResponse(new AsyncCallback(ReadHttpResponseCallback), webRequest);
                    }
                }
            }
            catch
            {
                CanCancel = false;
                Status = PhotoUploadStatus.Error;
            }
        }

        private void ReadHttpResponseCallback(IAsyncResult asynchronousResult)
        {
            Exception error = null;
            string newCoverURIs = string.Empty;
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult);
                
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    newCoverURIs = reader.ReadToEnd();
                    reader.Close();
                }
                Status = PhotoUploadStatus.Completed;
            }
            catch (Exception ex)
            {
                Status = PhotoUploadStatus.Error;
                error = ex;
            }
            finally
            {
                if (UploadCompleted != null)
                {
                    UploadCompleted(this, new PhotoUploadCompletedEventArgs(newCoverURIs, error));
                }
            }
        }
    }
}
