﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiteApp.MySpace.Web.Entities;

namespace LiteApp.MySpace.Web.DataAccess
{
    public interface IAlbumRepository
    {
        IEnumerable<Album> GetPagedAlbums(int pageIndex, int pageSize);
        Album FindAlbumById(string albumId);
        void SaveAlbum(Album album);
        void DeleteAlbum(string albumId);
        int GetTotalAlbumCount();
        string[] UpdateCover(string albumId, string coverUri);
        void UpdateCovers(Album album);
    }
}