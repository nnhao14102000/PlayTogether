using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using PlayTogether.Api.Helpers;
using System;
using System.IO;

namespace PlayTogether.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string currentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, reloadOnChange: true);

            if (currentEnvironment?.Equals("Development", StringComparison.OrdinalIgnoreCase) == true) {
                configurationBuilder.AddJsonFile($"appsettings.{currentEnvironment}.json", optional: false);
            }

            IConfigurationRoot config = configurationBuilder.Build();
            LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));
            Logger logger = LogManager.GetCurrentClassLogger();

            try {
                logger.Info($"{ApiConstants.FriendlyServiceName} starts running...");
                var host = CreateWebHostBuilder(args, config).Build();

                using (var scope = host.Services.CreateScope()) {
                    var services = scope.ServiceProvider;

                    try {
                        SeedData.Initialize(services);
                    }
                    catch (Exception ex) {
                        logger.Error(ex, "An error occurred seeding the DB.");
                    }
                }

                host.Run();
                logger.Info($"{ApiConstants.FriendlyServiceName} is stopped");
            }
            catch (Exception exception) {
                logger.Error(exception);
                throw;
            }
            finally {
                LogManager.Shutdown();
            }

        }

        public static IHostBuilder CreateWebHostBuilder(string[] args, IConfiguration configuration) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) => {
                    // var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

                    // if (!(bool)(appSettings?.ByPassKeyVault)) {
                    //     KeyVaultServices.GetAzureKeyVaultSecrets(context, config);
                    // }
                })
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging => {
                //logging.ClearProviders();
                logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog();
    }
}