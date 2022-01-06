using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Infrastructure.Entities;

namespace PlayTogether.Infrastructure.Helpers
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            // src => target
            CreateMap<RegisterDto, RegisterBasicInfoDto>();
            CreateMap<RegisterBasicInfoDto, Charity>();
            CreateMap<RegisterBasicInfoDto, Admin>();
            CreateMap<RegisterBasicInfoDto, Hirer>();
            CreateMap<RegisterBasicInfoDto, Player>();
        }
    }
}
