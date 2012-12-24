using System;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using AppLimit.CloudComputing.SharpBox.StorageProvider.DropBox;
using LiteApp.MySpace.Entities;
using LiteApp.MySpace.Web.DataAccess;
using LiteApp.MySpace.Web.Helpers;
using LiteApp.MySpace.Web.Shared;
using MongoDB.Bson;
using Ninject;
using LiteApp.MySpace.Web.Logging;

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

                var storage = SharpBoxSupport.OpenDropBoxStorage();
                var photoFolder = storage.EnsurePhotoFolder(parameters.AlbumId);
                var thumbFolder = storage.EnsureThumbFolder(parameters.AlbumId);
                var newFileName = ObjectId.GenerateNewId() + parameters.FileExtension;

                // Check request content length and actual stream length
                // If they're not equal, it means the client canceled the uploading
                bool lengthsMatched = context.Request.ContentLength == context.Request.InputStream.Length;
                if (!lengthsMatched)
                {
                    return;
                }

                // Compress photo and upload
                using (var img = Image.FromStream(context.Request.InputStream))
                {
                    using (var photoStream = img.ResampleAsStream(720, 445))
                    {
                        photoStream.Position = 0;
                        storage.UploadFile(photoStream, newFileName, photoFolder);
                    }
                }

                context.Request.InputStream.Position = 0; // Set stream starting position back for next read
                // Generate thumbnail and upload
                using (var img = Image.FromStream(context.Request.InputStream))
                {
                    using (var thumStream = img.ResampleAsStream(192, 119))
                    {
                        thumStream.Position = 0;
                        storage.UploadFile(thumStream, newFileName, thumbFolder);
                    }
                }

                // Retrieve photo and thumbnail URIs
                string thumbURI = DropBoxStorageProviderTools.GetPublicObjectUrl(storage.CurrentAccessToken,
                    storage.GetFileSystemObject(newFileName, thumbFolder)).AbsoluteUri;
                string photoURI = DropBoxStorageProviderTools.GetPublicObjectUrl(storage.CurrentAccessToken,
                    storage.GetFileSystemObject(newFileName, photoFolder)).AbsoluteUri;
                storage.Close();

                // Store data to database
                string[] coverURIs = AlbumRepository.UpdateCover(parameters.AlbumId, thumbURI);
                Photo photo = new Photo();
                photo.PhotoURI = photoURI;
                photo.ThumbURI = thumbURI;
                photo.AlbumId = parameters.AlbumId;
                photo.CreatedOn = DateTime.Now.ToUniversalTime();
                photo.CreatedBy = parameters.User;
                PhotoRepository.SavePhoto(photo);

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