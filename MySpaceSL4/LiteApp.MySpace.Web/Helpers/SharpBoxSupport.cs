using System.IO;
using AppLimit.CloudComputing.SharpBox;
using AppLimit.CloudComputing.SharpBox.Exceptions;
using LiteApp.MySpace.Web.Resources;

namespace LiteApp.MySpace.Web.Helpers
{
    public static class SharpBoxSupport
    {
        public static void DeleteAlbum(this CloudStorage storage, string albumId)
        {
            try
            {
                storage.DeleteFileSystemEntry(GetPhotoVirtualPath(albumId));
                storage.DeleteFileSystemEntry(GetThumbVirtualPath(albumId));
                storage.DeleteFileSystemEntry(GetDownloadVirtualPath(albumId));
            }
            catch (SharpBoxException ex)
            {
                if (ex.ErrorCode != SharpBoxErrorCodes.ErrorCouldNotRetrieveDirectoryList)
                    throw;
            }
        }

        public static ICloudDirectoryEntry EnsureThumbFolder(this CloudStorage storage, string albumId)
        {
            return storage.GetFolderEx(SharpBoxSupport.GetThumbVirtualPath(albumId));
        }

        public static ICloudDirectoryEntry EnsurePhotoFolder(this CloudStorage storage, string albumId)
        {
            return storage.GetFolderEx(SharpBoxSupport.GetPhotoVirtualPath(albumId));
        }

        public static ICloudDirectoryEntry EnsureDownloadFolder(this CloudStorage storage, string albumId)
        {
            return storage.GetFolderEx(SharpBoxSupport.GetDownloadVirtualPath(albumId));
        }

        public static void CreateFoldersForAlbum(this CloudStorage storage, string albumId)
        {
            storage.CreateFolder(SharpBoxSupport.GetThumbVirtualPath(albumId));
            storage.CreateFolder(SharpBoxSupport.GetPhotoVirtualPath(albumId));
            storage.CreateFolder(SharpBoxSupport.GetDownloadVirtualPath(albumId));
        }

        public static void DeletePhoto(this CloudStorage storage, string fileName, string albumId)
        {
            var thumbPath = GetThumbVirtualPath(albumId) + "/" + fileName;
            var photoPath = GetPhotoVirtualPath(albumId) + "/" + fileName;
            var downloadPath = GetDownloadVirtualPath(albumId) + "/" + fileName;
            storage.DeleteFileSystemEntry(thumbPath);
            storage.DeleteFileSystemEntry(photoPath);
            storage.DeleteFileSystemEntry(downloadPath);
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

        static string GetThumbVirtualPath(string albumId)
        {
            return "/Public/MySpace/Thumbs/" + albumId;
        }

        static string GetPhotoVirtualPath(string albumId)
        {
            return "/Public/MySpace/Photos/" + albumId;
        }

        static string GetDownloadVirtualPath(string albumId)
        {
            return "/Public/MySpace/Download/" + albumId;
        }

        static ICloudDirectoryEntry GetFolderEx(this CloudStorage storage, string path)
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
    }
}