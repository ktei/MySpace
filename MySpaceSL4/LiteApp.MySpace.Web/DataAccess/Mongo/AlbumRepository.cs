using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver.Builders;
using MongoDB.Bson;
using LiteApp.MySpace.Entities;
using LiteApp.MySpace.Web.DataAccess.Mongo.PO;

namespace LiteApp.MySpace.Web.DataAccess.Mongo
{
    public class AlbumRepository : BaseRepository, IAlbumRepository
    {
        public AlbumRepository()
        {
            Database.GetCollection<Album>(Collections.Albums).EnsureIndex("CreatedOn");
        }

        public IEnumerable<Album> GetPagedAlbums(int pageIndex, int pageSize)
        {
            var cursor = Database.GetCollection<AlbumPO>(Collections.Albums).FindAllAs<AlbumPO>();
            cursor.SetSortOrder(SortBy.Descending("CreatedOn")).SetSkip(pageIndex * pageSize).SetLimit(pageSize);
            foreach (var album in cursor)
            {
                yield return album.ToAlbum();
            }
        }

        public Album FindAlbumById(string albumId)
        {
            ObjectId albumObjectId;
            if (ObjectId.TryParse(albumId, out albumObjectId))
            {
                var albumPO = Database.GetCollection<AlbumPO>(Collections.Albums).FindOneByIdAs<AlbumPO>(albumObjectId);
                if (albumPO != null)
                    return albumPO.ToAlbum();
            }
            return null;
        }

        public string SaveAlbum(Album album)
        {
            AlbumPO albumPO = album.ToAlbumPO();
            if (albumPO.CoverURIs == null)
                albumPO.CoverURIs = new string[] { };
            if (string.IsNullOrEmpty(album.Id)) // Don't update this property if the entity is not new
                albumPO.CreatedOn = DateTime.Now.ToUniversalTime();
            Database.GetCollection<AlbumPO>(Collections.Albums).Save(albumPO);
            return albumPO.Id;
        }

        public void DeleteAlbum(string albumId)
        {
            ObjectId albumObjectId;
            if (ObjectId.TryParse(albumId, out albumObjectId))
            {
                // Delete related photo comments
                Database.GetCollection<PhotoCommentPO>(Collections.PhotoComments).Remove(Query.EQ("AlbumId", albumObjectId));
                // Delete related photos
                Database.GetCollection<PhotoPO>(Collections.Photos).Remove(Query.EQ("AlbumId", albumObjectId));
                // Delete album itself
                Database.GetCollection<AlbumPO>(Collections.Albums).Remove(Query.EQ("_id", albumObjectId));
            }
        }

        public string[] UpdateCover(string albumId, string coverUri)
        {
            ObjectId albumObjectId;
            if (ObjectId.TryParse(albumId, out albumObjectId))
            {
                Database.GetCollection<AlbumPO>(Collections.Albums).Update(Query.And(Query.EQ("_id", albumObjectId), Query.Size("CoverURIs", 3)), Update.PopFirst("CoverURIs"));
                Database.GetCollection<AlbumPO>(Collections.Albums).Update(Query.EQ("_id", albumObjectId), Update.AddToSet("CoverURIs", coverUri));
            }
            return Database.GetCollection<AlbumPO>(Collections.Albums).FindOneAs<AlbumPO>(Query.EQ("_id", albumObjectId)).CoverURIs;
        }

        public int GetTotalAlbumCount()
        {
            return Convert.ToInt32(Database.GetCollection<AlbumPO>(Collections.Albums).Count());
        }

        public string[] UpdateCovers(Album album)
        {
            ObjectId albumObjectId;
            string[] covers = new string[] { };
            if (ObjectId.TryParse(album.Id, out albumObjectId))
            {
                var cursor = Database.GetCollection<PhotoPO>(Collections.Photos).FindAs<PhotoPO>(Query.EQ("AlbumId", albumObjectId)).SetLimit(3);
                covers = cursor.Select(x => x.ThumbURI).ToArray();
                AlbumPO albumPO = album.ToAlbumPO();
                albumPO.CoverURIs = covers;
                Database.GetCollection<AlbumPO>(Collections.Albums).Save(albumPO);
            }
            return covers;
        }
    }
}