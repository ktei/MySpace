using System;

namespace LiteApp.MySpace.Entities
{
    public class Photo
    {
        public string Id { get; set; }

        public string AlbumId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string Description { get; set; }

        public string DownloadURI { get; set; }

        public string PhotoURI { get; set; }

        public string ThumbURI { get; set; }

        public int CommentCount { get; set; }
    }
}
