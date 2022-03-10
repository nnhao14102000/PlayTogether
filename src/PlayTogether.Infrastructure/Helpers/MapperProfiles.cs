using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.Chat;
using PlayTogether.Core.Dtos.Incoming.Business.Game;
using PlayTogether.Core.Dtos.Incoming.Business.GameOfPlayer;
using PlayTogether.Core.Dtos.Incoming.Business.GameType;
using PlayTogether.Core.Dtos.Incoming.Business.Hirer;
using PlayTogether.Core.Dtos.Incoming.Business.Image;
using PlayTogether.Core.Dtos.Incoming.Business.Music;
using PlayTogether.Core.Dtos.Incoming.Business.MusicOfPlayer;
using PlayTogether.Core.Dtos.Incoming.Business.Order;
using PlayTogether.Core.Dtos.Incoming.Business.Player;
using PlayTogether.Core.Dtos.Incoming.Business.Rank;
using PlayTogether.Core.Dtos.Incoming.Business.Rating;
using PlayTogether.Core.Dtos.Incoming.Business.Report;
using PlayTogether.Core.Dtos.Incoming.Business.TypeOfGame;
using PlayTogether.Core.Dtos.Outcoming.Business.Admin;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Business.Chat;
using PlayTogether.Core.Dtos.Outcoming.Business.Game;
using PlayTogether.Core.Dtos.Outcoming.Business.GameOfPlayer;
using PlayTogether.Core.Dtos.Outcoming.Business.GameType;
using PlayTogether.Core.Dtos.Outcoming.Business.Hirer;
using PlayTogether.Core.Dtos.Outcoming.Business.Image;
using PlayTogether.Core.Dtos.Outcoming.Business.Music;
using PlayTogether.Core.Dtos.Outcoming.Business.MusicOfPlayer;
using PlayTogether.Core.Dtos.Outcoming.Business.Notification;
using PlayTogether.Core.Dtos.Outcoming.Business.Order;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Business.Rating;
using PlayTogether.Core.Dtos.Outcoming.Business.Report;
using PlayTogether.Core.Dtos.Outcoming.Business.TypeOfGame;
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
            CreateMap<Hirer, HirerGetByIdResponse>();
            CreateMap<HirerInfoUpdateRequest, Hirer>();
            CreateMap<HirerStatusUpdateRequest, Hirer>();

            CreateMap<Charity, CharityResponse>();

            CreateMap<Player, PlayerGetAllResponseForHirer>();
            CreateMap<Player, PlayerGetProfileResponse>();
            CreateMap<Player, PlayerGetByIdResponseForPlayer>();
            CreateMap<Player, PlayerGetByIdResponseForHirer>();
            CreateMap<Player, PlayerServiceInfoResponseForPlayer>();
            CreateMap<Player, PlayerOtherSkillResponse>();
            CreateMap<Player, PlayerGetAllResponseForAdmin>();
            CreateMap<OtherSkillUpdateRequest, Player>();
            
            CreateMap<Image, ImagesOfPlayer>();
            CreateMap<PlayerInfoUpdateRequest, Player>();
            CreateMap<PlayerServiceInfoUpdateRequest, Player>();

            CreateMap<ImageCreateRequest, Image>();
            CreateMap<Image, ImageGetByIdResponse>();

            CreateMap<GameType, GameTypeGetAllResponse>();
            CreateMap<GameType, GameTypeGetByIdResponse>();
            CreateMap<GameTypeUpdateRequest, GameType>();
            CreateMap<GameTypeCreateRequest, GameType>();
            CreateMap<GameType, GameTypeCreateResponse>();
            CreateMap<TypeOfGame, TypeOfGameResponseForGameType>();
            CreateMap<Game, GameResponseForGameType>();

            CreateMap<Game, GameGetAllResponse>();
            CreateMap<Game, GameGetByIdResponse>();
            CreateMap<GameUpdateRequest, Game>();
            CreateMap<GameCreateRequest, Game>();
            CreateMap<Game, GameCreateResponse>();
            CreateMap<TypeOfGame, TypeOfGameResponseForGame>();
            CreateMap<GameType, GameTypeResponseForGame>();
            CreateMap<Rank, RankResponseForGame>();

            CreateMap<TypeOfGameCreateRequest, TypeOfGame>();
            CreateMap<TypeOfGame, TypeOfGameGetByIdResponse>();

            CreateMap<Rank, RankGetByIdResponse>();
            CreateMap<RankUpdateRequest, Rank>();
            CreateMap<RankCreateRequest, Rank>();
            CreateMap<Rank, RankCreateResponse>();

            CreateMap<MusicCreateRequest, Music>();
            CreateMap<MusicUpdateRequest, Music>();
            CreateMap<Music, MusicGetByIdResponse>();

            CreateMap<GameOfPlayerCreateRequest, GameOfPlayer>();
            CreateMap<GameOfPlayerUpdateRequest, GameOfPlayer>();
            CreateMap<GameOfPlayer, GameOfPlayerGetByIdResponse>();
            CreateMap<GameOfPlayer, GamesInPlayerGetAllResponse>();

            CreateMap<MusicOfPlayerCreateRequest, MusicOfPlayer>();
            CreateMap<MusicOfPlayer, MusicOfPlayerGetByIdResponse>();
            CreateMap<MusicOfPlayer, MusicOfPlayerGetAllResponse>();

            CreateMap<OrderCreateRequest, Order>();
            CreateMap<Order, OrderGetResponse>();
            CreateMap<Player, PlayerOrderResponse>();
            CreateMap<Hirer, HirerOrderResponse>();
            CreateMap<Order, OrderGetDetailResponse>();
            CreateMap<Rating, RatingInOrderResponse>();
            CreateMap<Report, ReportInOrderResponse>();

            CreateMap<Notification, NotificationGetDetailResponse>();
            CreateMap<Notification, NotificationGetAllResponse>();
            
            CreateMap<RatingCreateRequest, Rating>();
            CreateMap<Rating, RatingGetResponse>();
            CreateMap<Hirer, HirerRateResponse>();

            CreateMap<ReportCreateRequest, Report>();
            CreateMap<Report, ReportGetResponse>();
            CreateMap<Report, ReportInDetailResponse>();
            CreateMap<ReportCheckRequest, Report>();


            CreateMap<Chat, ChatGetResponse>();
            CreateMap<ChatCreateRequest, Chat>();

            CreateMap<Donate, DonateInOrderResponse>();

            

            // src => target
        }
    }
}
