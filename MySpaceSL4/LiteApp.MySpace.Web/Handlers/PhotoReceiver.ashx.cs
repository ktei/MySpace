using System;
using System.IO;
using System.Web;
using AppLimit.CloudComputing.SharpBox.StorageProvider.DropBox;
using LiteApp.MySpace.Web.Helpers;
using LiteApp.MySpace.Web.Services;
using MongoDB.Bson;
using System.Drawing;
using LiteApp.MySpace.Web.Helpers;

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
            var storage = SharpBoxSupport.OpenDropBoxStorage();
            var photoFolder = storage.GetFolderEx(GetPhotoVirtualPath());
            var thumbFolder = storage.GetFolderEx(GetThumbVirtualPath());
            var extension = context.Request.QueryString["extension"];
            var albumId = context.Request.QueryString["albumId"];
            if (string.IsNullOrEmpty(extension))
            {
                throw new Exception("No file extension specified.");
            }

            var newFileName = ObjectId.GenerateNewId() + extension.ToLower();
            // Save original file
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
            storage.Close();

            PhotoService svc = new PhotoService();
            svc.UpdateAlbumCover(albumId, thumbUri);
            //using (var stream = context.Request.InputStream)
            //{
            //    byte[] buffer = new Byte[stream.Length];
            //    stream.Read(buffer, 0, buffer.Length);
            //}

            context.Response.Write("OK");
        }

        #endregion

        private static string GetPhotoVirtualPath()
        {
            return "/Public/MySpace/Photos";
        }

        private static string GetThumbVirtualPath()
        {
            return "/Public/MySpace/Thumbs";
        }
    }
}