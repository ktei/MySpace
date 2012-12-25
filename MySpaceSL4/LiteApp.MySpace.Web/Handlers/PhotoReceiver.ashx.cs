using System;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using AppLimit.CloudComputing.SharpBox;
using AppLimit.CloudComputing.SharpBox.StorageProvider.DropBox;
using LiteApp.MySpace.Entities;
using LiteApp.MySpace.Web.DataAccess;
using LiteApp.MySpace.Web.Helpers;
using LiteApp.MySpace.Web.Logging;
using LiteApp.MySpace.Web.Shared;
using MongoDB.Bson;
using Ninject;

namespace LiteApp.MySpace.Web.Handlers
{
    public class PhotoReceiver : IHttpHandler, IRequiresSessionState
    {
        public PhotoReceiver()
        {
            DI.Inject(this);
        }

        [Inject]
        public IAlbumRepository AlbumRepository { get; set; }

        [Inject]
        public IPhotoRepository PhotoRepository { get; set; }

        [Inject]
        public IPhotoUploadTicketPool PhotoUploadTickets { get; set; }

        [Inject]
        public ILogger Logger { get; set; }

        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                // Prepare parameters
                UploadParameters parameters = GetParametersFromRequest(context);
                if (!VerifyTicket(parameters.RequestToken, parameters.Ticket))
                    throw new Exception("No valid ticket for this request was found.");

                // Check request content length and actual stream length
                // If they're not equal, it means the client canceled the uploading
                bool lengthsMatched = context.Request.ContentLength == context.Request.InputStream.Length;
                if (!lengthsMatched)
                {
                    return;
                }

                var storage = SharpBoxSupport.OpenDropBoxStorage();
                var newFileName = ObjectId.GenerateNewId() + parameters.FileExtension;
                string downloadURI = UploadDownlaodable(context, storage, newFileName, parameters.AlbumId);
                string photoURI = UploadPhoto(context, storage, newFileName, parameters.AlbumId);
                string thumbURI = UploadThumbnail(context, storage, newFileName, parameters.AlbumId);
                storage.Close();

                string[] coverURIs = AlbumRepository.UpdateCover(parameters.AlbumId, thumbURI);
                Photo photo = new Photo()
                {
                    PhotoURI = photoURI,
                    ThumbURI = thumbURI,
                    DownloadURI = downloadURI,
                    AlbumId = parameters.AlbumId,
                    CreatedBy = parameters.User
                };
                PhotoRepository.CreatePhoto(photo);
                context.Response.Write(string.Join(";", coverURIs));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                context.Request.InputStream.Close();
                context.Response.Write("error");
            }
        }

        #endregion

        string UploadDownlaodable(HttpContext context, CloudStorage storage, string newFileName, string albumId)
        {
            var downloadFolder = storage.EnsureDownloadFolder(albumId);
            using (var img = Image.FromStream(context.Request.InputStream))
            {
                using (var photoStream = img.ResampleAsStream(img.Width, img.Height))
                {
                    photoStream.Position = 0;
                    storage.UploadFile(photoStream, newFileName, downloadFolder);
                }
            }
            return DropBoxStorageProviderTools.GetPublicObjectUrl(storage.CurrentAccessToken,
                    storage.GetFileSystemObject(newFileName, downloadFolder)).AbsoluteUri;
        }

        string UploadPhoto(HttpContext context, CloudStorage storage, string newFileName, string albumId)
        {
            context.Request.InputStream.Position = 0;
            var photoFolder = storage.EnsurePhotoFolder(albumId);
            using (var img = Image.FromStream(context.Request.InputStream))
            {
                using (var photoStream = img.ResampleAsStream(720, 445))
                {
                    photoStream.Position = 0;
                    storage.UploadFile(photoStream, newFileName, photoFolder);
                }
            }
            return DropBoxStorageProviderTools.GetPublicObjectUrl(storage.CurrentAccessToken,
                    storage.GetFileSystemObject(newFileName, photoFolder)).AbsoluteUri;
        }

        string UploadThumbnail(HttpContext context, CloudStorage storage, string newFileName, string albumId)
        {
            context.Request.InputStream.Position = 0;
            var thumbFolder = storage.EnsureThumbFolder(albumId);
            using (var img = Image.FromStream(context.Request.InputStream))
            {
                using (var thumStream = img.ResampleAsStream(192, 119))
                {
                    thumStream.Position = 0;
                    storage.UploadFile(thumStream, newFileName, thumbFolder);
                }
            }
            return DropBoxStorageProviderTools.GetPublicObjectUrl(storage.CurrentAccessToken,
                storage.GetFileSystemObject(newFileName, thumbFolder)).AbsoluteUri;
        }

        UploadParameters GetParametersFromRequest(HttpContext context)
        {
            var extension = context.Request.QueryString["extension"];
            var albumId = context.Request.QueryString["albumId"];
            var user = context.Request.QueryString["user"];
            var requestToken = context.Request.QueryString["requestToken"];
            var ticket = context.Request.QueryString["ticket"];

            if (string.IsNullOrEmpty(extension))
            {
                throw new Exception("No file extension specified.");
            }
            if (string.IsNullOrEmpty(albumId))
            {
                throw new Exception("No album ID specified.");
            }
            if (string.IsNullOrEmpty(user))
            {
                throw new Exception("No upload user specified.");
            }
            if (string.IsNullOrEmpty(requestToken))
            {
                throw new Exception("No request token specified.");
            }
            if (string.IsNullOrEmpty(ticket))
            {
                throw new Exception("No ticket specified.");
            }
            return new UploadParameters() { AlbumId = albumId, FileExtension = extension.ToLower(), RequestToken = requestToken, Ticket = ticket, User = user };
        }

        bool VerifyTicket(string requestToken, string ticket)
        {
            bool result = false;
            result = PhotoUploadTickets.VerifyTicket(requestToken, ticket);
            // Dump this ticket immediately as it's been verified
            PhotoUploadTickets.DestroyTicket(requestToken);
            return result;
        }

        private class UploadParameters
        {
            public string FileExtension { get; set; }
            public string AlbumId { get; set; }
            public string User { get; set; }
            public string RequestToken { get; set; }
            public string Ticket { get; set; }
        }
    }
}