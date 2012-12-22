using System;

namespace LiteApp.MySpace.Entities
{
    public class PhotoComment
    {
        public string Id { get; set; }

        public string PhotoId { get; set; }

        public string AlbumId { get; set; }

        public string Contents { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
