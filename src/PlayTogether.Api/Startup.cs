using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using PlayTogether.Api.Helpers;
using PlayTogether.Core.Dtos.Validators.Auth;
using PlayTogether.Infrastructure.Data;
using System;
using System.Text;

namespace PlayTogether.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation(fv => {
                fv.RegisterValidatorsFromAssemblyContaining<GoogleLoginRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<RegisterAdminInfoRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<RegisterCharityInfoRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<RegisterRequestValidator>();
                fv.RegisterValidatorsFromAssemblyContaining<RegisterUserInfoRequestValidator>();
            });

            services.ConfigureServiceInjection(Configuration);

            services.AddIdentity<IdentityUser, IdentityRole>(options => {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 8;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(auth => {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options => {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                        ValidateIssuerSigningKey = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.ConfigureSwagger();

            services.AddRouting(options => options.LowercaseUrls = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseHttpCodeAndLogMiddleware();
                app.UseHsts();
            }

            app.ConfigureSwagger(provider);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }



    }
}