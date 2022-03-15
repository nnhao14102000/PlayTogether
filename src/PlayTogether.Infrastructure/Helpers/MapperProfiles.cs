using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Game;
using PlayTogether.Core.Dtos.Incoming.Business.GameType;
using PlayTogether.Core.Dtos.Incoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Business.Game;
using PlayTogether.Core.Dtos.Outcoming.Business.GameType;
using PlayTogether.Core.Dtos.Outcoming.Business.Rank;
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

            // GameType mapper profiles
            CreateMap<GameTypeCreateRequest, GameType>();
            CreateMap<GameTypeUpdateRequest, GameType>();
            CreateMap<GameType, GameTypeCreateResponse>();
            CreateMap<GameType, GameTypeGetByIdResponse>();
            CreateMap<GameType, GameTypeGetAllResponse>();
            CreateMap<TypeOfGame, TypeOfGameResponseForGameType>();
            CreateMap<Game, GameResponseForGameType>();

            // Game mapper profiles
            CreateMap<GameCreateRequest, Game>();
            CreateMap<GameUpdateRequest, Game>();
            CreateMap<Game, GameCreateResponse>();
            CreateMap<Game, GameGetAllResponse>();
            CreateMap<Game, GameGetByIdResponse>();
            CreateMap<TypeOfGame, TypeOfGameResponseForGame>();
            CreateMap<GameType, GameTypeResponseForGame>();
            CreateMap<Rank, RankResponseForGame>();

            // Rank mapper profiles
            CreateMap<RankCreateRequest, Rank>();
            CreateMap<RankUpdateRequest, Rank>();
            CreateMap<Rank, RankGetByIdResponse>();
            CreateMap<Rank, RankGetAllResponse>();
            CreateMap<Rank, RankCreateResponse>();


            // src => target
        }
    }
}
