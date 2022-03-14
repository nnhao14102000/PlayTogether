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

            // Account mapper profiles
            CreateMap<RegisterCharityInfoRequest, Charity>();
            CreateMap<RegisterUserInfoRequest, AppUser>();

            // src => target
        }
    }
}
