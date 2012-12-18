using System;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using AppLimit.CloudComputing.SharpBox.StorageProvider.DropBox;
using LiteApp.MySpace.Web.DataAccess;
using LiteApp.MySpace.Web.Entities;
using LiteApp.MySpace.Web.Helpers;
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

        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            // Prepare parameters
            var extension = context.Request.QueryString["extension"];
            var albumId = context.Request.QueryString["albumId"];
            if (string.IsNullOrEmpty(extension))
            {
                throw new Exception("No file extension specified.");
            }
            if (string.IsNullOrEmpty(albumId))
            {
                throw new Exception("No album ID specified.");
            }
            if (!VerifyTicket(context))
                throw new Exception("No valid ticket for this request was found.");

            var storage = SharpBoxSupport.OpenDropBoxStorage();
            var photoFolder = storage.EnsurePhotoFolder(albumId);
            var thumbFolder = storage.EnsureThumbFolder(albumId);
            var newFileName = ObjectId.GenerateNewId() + extension.ToLower();

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
            string[] coverURIs = AlbumRepository.UpdateCover(albumId, thumbURI);
            Photo photo = new Photo();
            photo.PhotoURI = photoURI;
            photo.ThumbURI = thumbURI;
            photo.AlbumId = albumId;
            PhotoRepository.SavePhoto(photo);
            //using (var stream = context.Request.InputStream)
            //{
            //    byte[] buffer = new Byte[stream.Length];
            //    stream.Read(buffer, 0, buffer.Length);
            //}
            
            context.Response.Write(string.Join(";", coverURIs));
        }

        #endregion

        bool VerifyTicket(HttpContext context)
        {
            var requestToken = context.Request.QueryString["requestToken"];
            var ticket = context.Request.QueryString["ticket"];
            if (!string.IsNullOrEmpty(requestToken) && !string.IsNullOrEmpty(ticket))
            {
                bool result = false;
                result = PhotoUploadTickets.VerifyTicket(requestToken, ticket);
                // Dump this ticket immediately as it's been verified
                PhotoUploadTickets.DestroyTicket(requestToken);
                return result;
            }

            return false;
        }
    }
}