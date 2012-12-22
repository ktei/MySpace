using System;

namespace LiteApp.MySpace.Entities
{
    public class Album
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int PhotoCount { get; set; }

        public string[] CoverURIs { get; set; }

        public string Description { get; set; }
    }
}
