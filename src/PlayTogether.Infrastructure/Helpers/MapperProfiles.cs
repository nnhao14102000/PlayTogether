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

            // Get all service
            CreateMap<Admin, AdminResponse>();

            CreateMap<Hirer, HirerGetAllResponse>();
            CreateMap<Hirer, HirerProfileResponse>();
            CreateMap<Hirer, HirerGetByIdForHirerResponse>();
            CreateMap<HirerUpdateInfoRequest, Hirer>();

            CreateMap<Charity, CharityResponse>();

            CreateMap<Player, PlayerResponse>();
        }
    }
}
