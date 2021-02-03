using Entities = Album.Domain.Entities;
using Models = Album.Domain.Models;
using AutoMapper;

namespace Album.Persistence.Mapping
{
    public class NewMappingProfile: Profile
    {
        public NewMappingProfile()
        {
            CreateMap<Entities.VideoHomePage, Models.VideoHomePageDto>()
               .ReverseMap();
            CreateMap<Entities.Attachment, Models.AttachmentDto>()
              .ReverseMap();
            CreateMap<Entities.AttachmentType, Models.AttachmentTypeDto>()
              .ReverseMap();
        }
    }
}
