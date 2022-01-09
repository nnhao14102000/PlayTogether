using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Infrastructure.Entities;

namespace PlayTogether.Infrastructure.Helpers
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            // src => target

            // Auth
            CreateMap<RegisterAdminInfoDto, Admin>();
            CreateMap<RegisterCharityInfoDto, Charity>();
            CreateMap<RegisterUserInfoDto, Player>();
            CreateMap<RegisterUserInfoDto, Hirer>();

            // Get all service
            CreateMap<Admin, AdminResponseDto>();
            CreateMap<Hirer, HirerResponseDto>();
            CreateMap<Charity, CharityResponseDto>();
            CreateMap<Player, PlayerResponseDto>();
        }
    }
}
