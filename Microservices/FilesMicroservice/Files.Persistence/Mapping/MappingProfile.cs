using Entities = Files.Domain.Entities;
using Models = Files.Domain.Models;
using AutoMapper;

namespace Files.Persistence.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Attachment, Models.AttachmentDto>()
              .ReverseMap();
            CreateMap<Entities.AttachmentType, Models.AttachmentTypeDto>()
              .ReverseMap();
        }
    }
}
