using Album.Domain.CommonEntities;
using System;

namespace Album.Domain.Entities
{
    public class VideoHomePage : AuditableEntity
    {
        public Guid Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string YoutubeLink { get; set; }
        public string YoutubeId { get; set; }
        public string YoutubeImage { get; set; }
        public string Descriptions { get; set; }
        public string Code { get; set; }
    }
}
