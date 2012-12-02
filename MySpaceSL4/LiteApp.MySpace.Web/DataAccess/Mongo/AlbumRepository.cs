using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LiteApp.MySpace.Web.Entities;
using MongoDB.Driver.Builders;

namespace LiteApp.MySpace.Web.DataAccess.Mongo
{
    public class AlbumRepository : BaseRepository, IAlbumRepository
    {
        public IEnumerable<Album> GetAllAlbums()
        {
            return Database.GetCollection<Album>(Collections.Albums).FindAll();
        }

        public void SaveAlbum(Album album)
        {
            Database.GetCollection<Album>(Collections.Albums).Save(album);
        }

        public void DeleteAlbum(string albumId)
        {
            Database.GetCollection<Album>(Collections.Albums).Remove(Query.EQ("_id", albumId));
        }
    }
}