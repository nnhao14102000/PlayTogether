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
            CreateMap<RegisterAdminInfoDto, Admin>();
            CreateMap<RegisterCharityInfoDto, Charity>();
            CreateMap<RegisterUserInfoDto, Player>();
            CreateMap<RegisterUserInfoDto, Hirer>();
        }
    }
}
