using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Hirer;
using PlayTogether.Core.Dtos.Incoming.Business.Image;
using PlayTogether.Core.Dtos.Incoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Business.Image;
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
            CreateMap<Hirer, HirerGetProfileResponse>();
            CreateMap<Hirer, HirerGetByIdResponseForHirer>();
            CreateMap<HirerInfoUpdateRequest, Hirer>();

            CreateMap<Charity, CharityResponse>();

            CreateMap<Player, PlayerGetAllResponseForHirer>();
            CreateMap<Player, PlayerGetProfileResponse>();
            CreateMap<Player, PlayerGetByIdResponseForPlayer>();
            CreateMap<Player, PlayerGetByIdResponseForHirer>();
            CreateMap<Player, PlayerServiceInfoResponseForPlayer>();
            
            CreateMap<Image, ImagesOfPlayer>();
            CreateMap<PlayerInfoUpdateRequest, Player>();
            CreateMap<PlayerServiceInfoUpdateRequest, Player>();

            CreateMap<ImageCreateRequest, Image>();
            CreateMap<Image, ImageGetByIdResponse>();
        }
    }
}
