using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;

namespace Files.Domain.Models
{
    public class VideoHomePageDto
    {
        public Guid Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string YoutubeLink { get; set; }
        public string YoutubeId { get; set; }
        public string YoutubeImage { get; set; }
        public string Descriptions { get; set; }
        public string Code { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }

        [IgnoreMap]
        public IFormFile File { get; set; }
    }
}
