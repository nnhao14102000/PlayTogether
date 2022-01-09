using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayTogether.Core.Interfaces.Repositories.Auth;
using PlayTogether.Core.Interfaces.Repositories.Business.Admin;
using PlayTogether.Core.Interfaces.Repositories.Business.Charity;
using PlayTogether.Core.Interfaces.Repositories.Business.Hirer;
using PlayTogether.Core.Interfaces.Repositories.Business.Player;
using PlayTogether.Core.Interfaces.Services.Auth;
using PlayTogether.Core.Interfaces.Services.Business.Admin;
using PlayTogether.Core.Interfaces.Services.Business.Charity;
using PlayTogether.Core.Interfaces.Services.Business.Hirer;
using PlayTogether.Core.Interfaces.Services.Business.Player;
using PlayTogether.Core.Services.Auth;
using PlayTogether.Core.Services.Business.Admin;
using PlayTogether.Core.Services.Business.Charity;
using PlayTogether.Core.Services.Business.Hirer;
using PlayTogether.Core.Services.Business.Player;
using PlayTogether.Infrastructure.Data;
using PlayTogether.Infrastructure.Repositories.Auth;
using PlayTogether.Infrastructure.Repositories.Business.Admin;
using PlayTogether.Infrastructure.Repositories.Business.Charity;
using PlayTogether.Infrastructure.Repositories.Business.Hirer;
using PlayTogether.Infrastructure.Repositories.Business.Player;
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

            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            string playTogetherDbConnectionString = string.Empty;
            if ((bool)(appSettings?.ByPassKeyVault))// use for localhost
            {
                playTogetherDbConnectionString = configuration.GetConnectionString("PlayTogetherDbConnection");
            }
            else // use in environment
            {
                playTogetherDbConnectionString = GetSecret.PlayTogetherDbConnectionString().Result;
            }

            // Play together Db config
            services.AddDbContext<AppDbContext>
            (
                optionsAction: options => options.UseSqlServer(playTogetherDbConnectionString),
                contextLifetime: ServiceLifetime.Transient,
                optionsLifetime: ServiceLifetime.Transient
            );

            // Config for automapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Config for DI
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthRepository, AuthRepository>();

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

            services.AddHttpClient();

            return services;
        }
    }
}