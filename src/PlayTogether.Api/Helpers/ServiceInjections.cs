﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayTogether.Core.Interfaces.Repositories.Auth;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Auth;
using PlayTogether.Core.Interfaces.Services.Business;
using PlayTogether.Core.Services.Auth;
using PlayTogether.Core.Services.Business.Admin;
using PlayTogether.Core.Services.Business.AppUser;
using PlayTogether.Core.Services.Business.Charity;
using PlayTogether.Core.Services.Business.Chat;
using PlayTogether.Core.Services.Business.Donate;
using PlayTogether.Core.Services.Business.Game;
using PlayTogether.Core.Services.Business.GameOfPlayer;
using PlayTogether.Core.Services.Business.GameType;
using PlayTogether.Core.Services.Business.Hirer;
using PlayTogether.Core.Services.Business.Hobby;
using PlayTogether.Core.Services.Business.Image;
using PlayTogether.Core.Services.Business.Music;
using PlayTogether.Core.Services.Business.MusicOfPlayer;
using PlayTogether.Core.Services.Business.Notification;
using PlayTogether.Core.Services.Business.Order;
using PlayTogether.Core.Services.Business.Player;
using PlayTogether.Core.Services.Business.Rank;
using PlayTogether.Core.Services.Business.Rating;
using PlayTogether.Core.Services.Business.Report;
using PlayTogether.Core.Services.Business.TypeOfGame;
using PlayTogether.Infrastructure.Data;
using PlayTogether.Infrastructure.Repositories.Auth;
using PlayTogether.Infrastructure.Repositories.Business.Admin;
using PlayTogether.Infrastructure.Repositories.Business.AppUser;
using PlayTogether.Infrastructure.Repositories.Business.Charity;
using PlayTogether.Infrastructure.Repositories.Business.Chat;
using PlayTogether.Infrastructure.Repositories.Business.Donate;
using PlayTogether.Infrastructure.Repositories.Business.Game;
using PlayTogether.Infrastructure.Repositories.Business.GameOfPlayer;
using PlayTogether.Infrastructure.Repositories.Business.GameType;
using PlayTogether.Infrastructure.Repositories.Business.Hirer;
using PlayTogether.Infrastructure.Repositories.Business.Hobby;
using PlayTogether.Infrastructure.Repositories.Business.Image;
using PlayTogether.Infrastructure.Repositories.Business.Music;
using PlayTogether.Infrastructure.Repositories.Business.MusicOfPlayer;
using PlayTogether.Infrastructure.Repositories.Business.Notification;
using PlayTogether.Infrastructure.Repositories.Business.Order;
using PlayTogether.Infrastructure.Repositories.Business.Player;
using PlayTogether.Infrastructure.Repositories.Business.Rank;
using PlayTogether.Infrastructure.Repositories.Business.Rating;
using PlayTogether.Infrastructure.Repositories.Business.Report;
using PlayTogether.Infrastructure.Repositories.Business.TypeOfGame;
using System;

namespace PlayTogether.Api.Helpers
{
    public static class ServiceInjections
    {
        public static IServiceCollection ConfigureServiceInjection(this IServiceCollection services, IConfiguration configuration)
        {
            if (services is null) {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null) {
                throw new ArgumentNullException(nameof(configuration));
            }

            // var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            // string playTogetherDbConnectionString = string.Empty;
            // if ((bool)(appSettings?.ByPassKeyVault))// use for localhost
            // {
            //     playTogetherDbConnectionString = configuration.GetConnectionString("PlayTogetherDbConnection");
            // }
            // else // use in environment
            // {
            //     playTogetherDbConnectionString = GetSecret.PlayTogetherDbConnectionString().Result;
            // }

            // Play together Db config
            services.AddDbContext<AppDbContext>
            (
                // optionsAction: options => options.UseSqlServer(playTogetherDbConnectionString)
                options => options.UseSqlServer(configuration.GetConnectionString("PlayTogetherDbConnection"))
            );

            // Config for automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Config for DI
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            // Config for Admin service DI
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IAdminRepository, AdminRepository>();

            // Config for Charity service DI
            services.AddScoped<ICharityService, CharityService>();
            services.AddScoped<ICharityRepository, CharityRepository>();

            // Config for Player service DI
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();

            // Config for Hirer service DI
            services.AddScoped<IHirerService, HirerService>();
            services.AddScoped<IHirerRepository, HirerRepository>();

            // Config for Image service DI
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IImageRepository, ImageRepository>();

            // Config for GameType service DI
            services.AddScoped<IGameTypeService, GameTypeService>();
            services.AddScoped<IGameTypeRepository, GameTypeRepository>();

            // Config for Game service DI
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IGameRepository, GameRepository>();

            // Config for TypeOfGame service DI
            services.AddScoped<ITypeOfGameService, TypeOfGameService>();
            services.AddScoped<ITypeOfGameRepository, TypeOfGameRepository>();

            // Config for Rank service DI
            services.AddScoped<IRankService, RankService>();
            services.AddScoped<IRankRepository, RankRepository>();

            // Config for Music service DI
            services.AddScoped<IMusicService, MusicService>();
            services.AddScoped<IMusicRepository, MusicRepository>();

            // Config for GameOfPlayer service DI
            services.AddScoped<IGameOfPlayerService, GameOfPlayerService>();
            services.AddScoped<IGameOfPlayerRepository, GameOfPlayerRepository>();

            // Config for MusicOfPlayer service DI
            services.AddScoped<IMusicOfPlayerService, MusicOfPlayerService>();
            services.AddScoped<IMusicOfPlayerRepository, MusicOfPlayerRepository>();

            // Config for Order service DI
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            // Config for Notification service DI
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            // Config for Donate service DI
            services.AddScoped<IDonateService, DonateService>();
            services.AddScoped<IDonateRepository, DonateRepository>();

            // Config for Rating service DI
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IRatingRepository, RatingRepository>();

            // Config for Report service DI
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReportRepository, ReportRepository>();

            // Config for Chat service DI
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IChatRepository, ChatRepository>();

            // Config for Hobby service DI
            services.AddScoped<IHobbyService, HobbyService>();
            services.AddScoped<IHobbyRepository, HobbyRepository>();

            // Config for AppUser service DI
            services.AddScoped<IAppUserService, AppUserService>();
            services.AddScoped<IAppUserRepository, AppUserRepository>();

            services.AddHttpClient();

            return services;
        }
    }
}