using System;
using System.Drawing;
using System.Web;
using AppLimit.CloudComputing.SharpBox.StorageProvider.DropBox;
using LiteApp.MySpace.Web.Helpers;
using LiteApp.MySpace.Web.Services;
using MongoDB.Bson;
using LiteApp.MySpace.Web.Entities;

namespace LiteApp.MySpace.Web.Handlers
{
    public class PhotoReceiver : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>PhotoReceiver
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            var extension = context.Request.QueryString["extension"];
            var albumId = context.Request.QueryString["albumId"];
            if (string.IsNullOrEmpty(extension))
            {
                throw new ApplicationException("No file extension specified.");
            }
            if (string.IsNullOrEmpty(albumId))
            {
                throw new ApplicationException("No album ID specified.");
            }

            var storage = SharpBoxSupport.OpenDropBoxStorage();
            var photoFolder = storage.EnsurePhotoFolder(albumId);
            var thumbFolder = storage.EnsureThumbFolder(albumId);

            var newFileName = ObjectId.GenerateNewId() + extension.ToLower();
            storage.UploadFile(context.Request.InputStream, newFileName, photoFolder);
            context.Request.InputStream.Position = 0;
            
            using (var img = Image.FromStream(context.Request.InputStream))
            {
                using (var thumStream = img.ResampleAsStream(192, 119))
                {
                    thumStream.Position = 0;
                    storage.UploadFile(thumStream, newFileName, thumbFolder);
                }
            }

            string thumbUri = DropBoxStorageProviderTools.GetPublicObjectUrl(storage.CurrentAccessToken,
                storage.GetFileSystemObject(newFileName, thumbFolder)).AbsoluteUri;
            string photoUri = DropBoxStorageProviderTools.GetPublicObjectUrl(storage.CurrentAccessToken,
                storage.GetFileSystemObject(newFileName, photoFolder)).AbsoluteUri;
            storage.Close();

            PhotoService svc = new PhotoService();
            string[] coverURIs = svc.UpdateAlbumCover(albumId, thumbUri);
            Photo photo = new Photo();
            photo.PhotoUri = photoUri;
            photo.ThumbUri = thumbUri;
            photo.AlbumId = albumId;
            svc.SavePhoto(photo);
            //using (var stream = context.Request.InputStream)
            //{
            //    byte[] buffer = new Byte[stream.Length];
            //    stream.Read(buffer, 0, buffer.Length);
            //}

            context.Response.Write(string.Join(";", coverURIs));
        }

        #endregion
    }
}