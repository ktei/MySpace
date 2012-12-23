using System.Collections.Generic;
using LiteApp.MySpace.Entities;

namespace LiteApp.MySpace.Web.DataAccess
{
    public interface IAlbumRepository
    {
        IEnumerable<Album> GetPagedAlbums(int pageIndex, int pageSize);
        Album FindAlbumById(string albumId);
        string SaveAlbum(Album album);
        void DeleteAlbum(string albumId);
        int GetTotalAlbumCount();
        string[] UpdateCover(string albumId, string coverUri);
        string[] UpdateCovers(Album album);
    }
}