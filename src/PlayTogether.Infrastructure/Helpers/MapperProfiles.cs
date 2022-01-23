using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Hirer;
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
            CreateMap<RegisterAdminInfoRequest, Admin>();
            CreateMap<RegisterCharityInfoRequest, Charity>();
            CreateMap<RegisterUserInfoRequest, Player>();
            CreateMap<RegisterUserInfoRequest, Hirer>();

            // Business
            CreateMap<Admin, AdminResponse>();

            CreateMap<Hirer, HirerGetAllResponseForAdmin>();
            CreateMap<Hirer, HirerProfileResponse>();
            CreateMap<Hirer, HirerGetByIdResponseForHirer>();
            CreateMap<HirerUpdateInfoRequest, Hirer>();

            CreateMap<Charity, CharityResponse>();

            CreateMap<Player, PlayerGetAllResponseForHirer>();
            CreateMap<Player, PlayerProfileResponse>();
            CreateMap<Player, PlayerGetByIdResponseForPlayer>();
            CreateMap<PlayerUpdateInfoRequest, Player>();
        }
    }
}
