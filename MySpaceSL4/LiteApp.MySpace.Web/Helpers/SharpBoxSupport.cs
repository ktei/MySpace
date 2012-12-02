using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AppLimit.CloudComputing.SharpBox;
using AppLimit.CloudComputing.SharpBox.Exceptions;
using System.IO;
using LiteApp.MySpace.Web.Resources;

namespace LiteApp.MySpace.Web.Helpers
{
    public static class SharpBoxSupport
    {
        public static ICloudDirectoryEntry GetFolderEx(this CloudStorage storage, string path)
        {
            try
            {
                return storage.GetFolder(path);
            }
            catch (SharpBoxException ex)
            {
                if (ex.ErrorCode == SharpBoxErrorCodes.ErrorCouldNotRetrieveDirectoryList || ex.ErrorCode == SharpBoxErrorCodes.ErrorFileNotFound)
                {
                    return storage.CreateFolder(path);
                }
                else
                {
                    throw;
                }
            }
        }

        public static CloudStorage OpenDropBoxStorage()
        {
            // Creating the cloudstorage object 
            CloudStorage dropBoxStorage = new CloudStorage();
            // get the configuration for dropbox 
            var dropBoxConfig = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox);

            // declare an access token
            ICloudStorageAccessToken accessToken = null;
            // load a valid security token from file
            using (var tokenStream = new MemoryStream(SupportFiles.DropBoxToken))
            {
                accessToken = dropBoxStorage.DeserializeSecurityToken(tokenStream);
            }
            // open the connection 
            var storageToken = dropBoxStorage.Open(dropBoxConfig, accessToken);
            return dropBoxStorage;
        }
    }
}