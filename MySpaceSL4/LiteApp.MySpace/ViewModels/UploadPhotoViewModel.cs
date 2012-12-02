using System;
using System.IO;
using System.Net;
using System.Windows;
using Caliburn.Micro;
using System.Threading;

namespace LiteApp.MySpace.ViewModels
{
    public class UploadPhotoViewModel : Screen
    {
        string _baseUri;
        private Stream _fileStream;
        private long _dataLength;
        private long _dataSent;
        string _fileName;
        bool _canceled;
        bool _canCancel;
        string _status;
        double _progress;

        public event EventHandler UploadStarted;

        public event EventHandler UploadCanceled;

        public event EventHandler UploadCompleted;

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

        public string Status
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
            DisplayName = "Upload Photo";
            Uri fullUri = Application.Current.Host.Source;
            _baseUri = fullUri.AbsoluteUri.Substring(0, fullUri.AbsoluteUri.IndexOf("/ClientBin"));
        }

        public void StartUpload(FileInfo file)
        {
            _fileStream = file.OpenRead();
            _dataLength = _fileStream.Length;
            UriBuilder httpHandlerUrlBuilder = new UriBuilder(string.Format("{0}/Handlers/PhotoReceiver.ashx", _baseUri));

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(httpHandlerUrlBuilder.Uri);
            webRequest.Method = "POST";
            webRequest.BeginGetRequestStream(new AsyncCallback(WriteToStreamCallback), webRequest);
            FileName = file.Name;
            CanCancel = true;
            if (UploadStarted != null)
                UploadStarted(this, EventArgs.Empty);
        }

        public void CancelUpload()
        {
            CanCancel = false;
            Status = "Canceling...";
            _canceled = true;
        }

        private void WriteToStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
            Stream requestStream = webRequest.EndGetRequestStream(asynchronousResult);

            byte[] buffer = new Byte[4096];
            int bytesRead = 0;

            //Set the start position
            _fileStream.Position = 0;

            //Keep reading and flush to server
            //while ((bytesRead = _fileStream.Read(buffer, 0, buffer.Length)) != 0)
            //{
            //    if (_canceled)
            //    {
            //        Status = "Canceled";
            //        break;
            //    }

            //    requestStream.Write(buffer, 0, bytesRead);
            //    requestStream.Flush();
            //    _dataSent += bytesRead;
            //    Progress = (double)_dataSent / (double)_dataLength;
            //}

            //if (_canceled)
            //{
            //    Thread.Sleep(1000); // Pause 1 sec for user to read 'Canceled' message
            //    if (UploadCanceled != null)
            //        UploadCanceled(this, EventArgs.Empty);
            //}
            //else
            //{
            //    CanCancel = false;
            //    Status = "Completing...";
            //    //Get the response from the HttpHandler
            //    requestStream.Close();
            //    webRequest.BeginGetResponse(new AsyncCallback(ReadHttpResponseCallback), webRequest);
            //}

            while ((bytesRead = _fileStream.Read(buffer, 0, buffer.Length)) != 0)
            {
                requestStream.Write(buffer, 0, bytesRead);
                requestStream.Flush();

                _dataSent += bytesRead;

                    Progress = (double)_dataSent / (double)_dataLength;
            }

            requestStream.Close();

            //Get the response from the HttpHandler
            webRequest.BeginGetResponse(new AsyncCallback(ReadHttpResponseCallback), webRequest);
        }

        private void ReadHttpResponseCallback(IAsyncResult asynchronousResult)
        {

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult);
                StreamReader reader = new StreamReader(webResponse.GetResponseStream());

                string responsestring = reader.ReadToEnd();
                reader.Close();
                Status = "Completed";
                Thread.Sleep(1000); // Pause 1 sec for user to read 'Canceled' message
                if (UploadCompleted != null)
                    UploadCompleted(this, EventArgs.Empty);
            }
            catch
            {

            }

            _fileStream.Close();
            _fileStream.Dispose();
        }
    }
}
