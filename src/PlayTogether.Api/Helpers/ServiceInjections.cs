using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayTogether.Core.Interfaces.Repositories.Auth;
using PlayTogether.Core.Interfaces.Services.Auth;
using PlayTogether.Core.Services.Auth;
using PlayTogether.Infrastructure.Data;
using PlayTogether.Infrastructure.Repositories.Auth;
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

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddHttpClient();

            return services;
        }
    }
}