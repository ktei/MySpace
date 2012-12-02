using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiteApp.MySpace.Web.Helpers;
using AppLimit.CloudComputing.SharpBox.StorageProvider.DropBox;

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
            //var storage = SharpBoxSupport.OpenDropBoxStorage();
            //var folder = storage.GetFolderEx(GetPhotoVirtualPath());
            //var fileName = Guid.NewGuid() + ".png";
            //storage.UploadFile(context.Request.InputStream, fileName, folder);

            //string photoUri = DropBoxStorageProviderTools.GetPublicObjectUrl(storage.CurrentAccessToken, storage.GetFileSystemObject(fileName, folder)).AbsoluteUri;
            //storage.Close();

            context.Response.Write("OK");
        }

        #endregion

        private static string GetPhotoVirtualPath()
        {
            return "/Public/MySpace/Photos";
        }
    }
}