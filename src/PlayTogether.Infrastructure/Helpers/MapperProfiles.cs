using System;
using AutoMapper;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Business.AppUser;
using PlayTogether.Core.Dtos.Incoming.Business.Charity;
using PlayTogether.Core.Dtos.Incoming.Business.Chat;
using PlayTogether.Core.Dtos.Incoming.Business.Dating;
using PlayTogether.Core.Dtos.Incoming.Business.Donate;
using PlayTogether.Core.Dtos.Incoming.Business.Game;
using PlayTogether.Core.Dtos.Incoming.Business.GameOfUser;
using PlayTogether.Core.Dtos.Incoming.Business.GameType;
using PlayTogether.Core.Dtos.Incoming.Business.Hobby;
using PlayTogether.Core.Dtos.Incoming.Business.Image;
using PlayTogether.Core.Dtos.Incoming.Business.Notification;
using PlayTogether.Core.Dtos.Incoming.Business.Order;
using PlayTogether.Core.Dtos.Incoming.Business.Rank;
using PlayTogether.Core.Dtos.Incoming.Business.Rating;
using PlayTogether.Core.Dtos.Incoming.Business.Report;
using PlayTogether.Core.Dtos.Incoming.Business.SystemConfig;
using PlayTogether.Core.Dtos.Incoming.Business.SystemFeedback;
using PlayTogether.Core.Dtos.Incoming.Business.TypeOfGame;
using PlayTogether.Core.Dtos.Outcoming.Business.AppUser;
using PlayTogether.Core.Dtos.Outcoming.Business.Charity;
using PlayTogether.Core.Dtos.Outcoming.Business.Chat;
using PlayTogether.Core.Dtos.Outcoming.Business.Donate;
using PlayTogether.Core.Dtos.Outcoming.Business.Game;
using PlayTogether.Core.Dtos.Outcoming.Business.GameOfUser;
using PlayTogether.Core.Dtos.Outcoming.Business.GameType;
using PlayTogether.Core.Dtos.Outcoming.Business.Hobby;
using PlayTogether.Core.Dtos.Outcoming.Business.Image;
using PlayTogether.Core.Dtos.Outcoming.Business.Notification;
using PlayTogether.Core.Dtos.Outcoming.Business.Order;
using PlayTogether.Core.Dtos.Outcoming.Business.Rank;
using PlayTogether.Core.Dtos.Outcoming.Business.Rating;
using PlayTogether.Core.Dtos.Outcoming.Business.Report;
using PlayTogether.Core.Dtos.Outcoming.Business.SearchHistory;
using PlayTogether.Core.Dtos.Outcoming.Business.SystemFeedback;
using PlayTogether.Core.Dtos.Outcoming.Business.TransactionHistory;
using PlayTogether.Core.Dtos.Outcoming.Business.TypeOfGame;
using PlayTogether.Core.Dtos.Outcoming.Business.UnActiveBalance;
using PlayTogether.Core.Entities;

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

            // TypeOfGame mapper profile
            CreateMap<TypeOfGameCreateRequest, TypeOfGame>();
            CreateMap<TypeOfGame, TypeOfGameGetByIdResponse>();

            // Hobby mapper profile
            CreateMap<HobbyCreateRequest, Hobby>();
            CreateMap<Hobby, HobbiesGetAllResponse>();
            CreateMap<AppUser, UserHobbyResponse>();
            CreateMap<Game, GameHobbyResponse>();

            // AppUser mapper profile
            CreateMap<AppUser, PersonalInfoResponse>();
            CreateMap<Image, ImageUserResponse>();
            CreateMap<UserBalance, UserBalanceResponse>();
            CreateMap<BehaviorPoint, BehaviorPointResponse>();

            CreateMap<UserPersonalInfoUpdateRequest, AppUser>();
            CreateMap<UserIsPlayerChangeRequest, AppUser>();
            CreateMap<UserInfoForIsPlayerUpdateRequest, AppUser>();

            CreateMap<AppUser, UserGetBasicInfoResponse>();
            CreateMap<AppUser, UserGetServiceInfoResponse>();
            CreateMap<AppUser, UserSearchResponse>();
            CreateMap<AppUser, UserGetByAdminResponse>();

            // Game Of User / Player mapper profile
            CreateMap<GameOfUser, GamesOfUserResponse>();
            CreateMap<GameOfUser, GameOfUserGetByIdResponse>();
            CreateMap<Game, GameGetAllResponse>();

            CreateMap<GameOfUserCreateRequest, GameOfUser>();
            CreateMap<GameOfUserUpdateRequest, GameOfUser>();

            // SearchHistory mapper profile
            CreateMap<SearchHistory, SearchHistoryResponse>();

            // Order mapper profile
            CreateMap<Order, OrderGetResponse>();
            CreateMap<AppUser, OrderUserResponse>();
            CreateMap<OrderCreateRequest, Order>();
            CreateMap<GameOfOrder, GameOfOrderResponse>();
            CreateMap<Game, GameResponse>();
            CreateMap<GamesOfOrderCreateRequest, GameOfOrder>();
            CreateMap<Rating, RatingInOrderResponse>();
            CreateMap<Report, ReportInOrderResponse>();
            CreateMap<Order, OrderGetDetailResponse>();


            // Chat mapper profile
            CreateMap<ChatCreateRequest, Chat>();
            CreateMap<Chat, ChatGetResponse>();

            // Notification mapper profile
            CreateMap<Notification, NotificationGetAllResponse>();
            CreateMap<Notification, NotificationGetDetailResponse>();
            CreateMap<NotificationCreateRequest, Notification>();
            CreateMap<NotificationCreateAllServerRequest, Notification>();

            // Rating mapper profile
            CreateMap<Rating, RatingGetResponse>();
            CreateMap<AppUser, UserRateResponse>();
            CreateMap<Rating, RatingGetDetailResponse>();
            CreateMap<RatingCreateRequest, Rating>();

            // Report mapper profile
            CreateMap<Report, ReportGetResponse>();
            CreateMap<ReportCreateRequest, Report>();
            CreateMap<ReportCheckRequest, Report>();
            CreateMap<Report, ReportInDetailResponse>();

            // Image mapper profile
            CreateMap<Image, ImageGetByIdResponse>();
            CreateMap<ImageCreateRequest, Image>();

            // TransactionHistory mapper Profile
            CreateMap<TransactionHistory, TransactionHistoryResponse>();

            // UnActiveBalance mapper Profile
            CreateMap<UnActiveBalance, UnActiveBalanceResponse>();

            // Charity mapper Profile
            CreateMap<Charity, CharityResponse>();
            CreateMap<CharityStatusRequest, Charity>();
            CreateMap<CharityUpdateRequest, Charity>();

            // Donate mapper Profile
            CreateMap<DonateCreateRequest, Donate>();
            CreateMap<Donate, DonateResponse>();

            // DisableUser mapper Profile
            CreateMap<DisableUser, DisableUserResponse>();

            // SystemFeedback mapper Profile
            CreateMap<ProcessFeedbackRequest, SystemFeedback>();
            CreateMap<CreateFeedbackRequest, SystemFeedback>();
            CreateMap<UpdateFeedbackRequest, SystemFeedback>();
            CreateMap<SystemFeedback, SystemFeedbackDetailResponse>();
            CreateMap<SystemFeedback, SystemFeedbackResponse>();

            // Dating mapper profile
            CreateMap<DatingCreateRequest, Dating>();
            CreateMap<DatingUpdateRequest, Dating>();
            CreateMap<Dating, DatingUserResponse>();

            // Charity with draw
            CreateMap<CharityWithDrawRequest, CharityWithdraw>();

            // System config
            CreateMap<ConfigCreateRequest, SystemConfig>();
            CreateMap<ConfigUpdateRequest, SystemConfig>();

            // src => target
        }
    }
}
