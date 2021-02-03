using UserManagement.Application.Common.Results;
using UserManagement.Domain.Common;

namespace UserManagement.Application.Common.Mappings
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserResult>().ReverseMap();
        }
    }
}
