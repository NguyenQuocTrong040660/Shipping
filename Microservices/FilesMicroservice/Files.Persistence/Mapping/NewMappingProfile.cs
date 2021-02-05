using Entities = Files.Domain.Entities;
using Models = Files.Domain.Models;
using AutoMapper;

namespace Files.Persistence.Mapping
{
    public class NewMappingProfile: Profile
    {
        public NewMappingProfile()
        {
            CreateMap<Entities.Attachment, Models.AttachmentDto>()
              .ReverseMap();
            CreateMap<Entities.AttachmentType, Models.AttachmentTypeDto>()
              .ReverseMap();
        }
    }
}
