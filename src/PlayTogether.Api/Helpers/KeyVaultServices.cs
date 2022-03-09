using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Api.Helpers
{
    public static class KeyVaultServices
    {
        public static string BaseUri { get; set; }

        private static KeyVaultClient _KeyVaultClient = null;

        public static KeyVaultClient KeyVaultClient {
            get {
                if (_KeyVaultClient is null) {
                    var provider = new AzureServiceTokenProvider();
                    _KeyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(provider.KeyVaultTokenCallback));
                }
                return _KeyVaultClient;
            }
        }

        public static void GetAzureKeyVaultSecrets(HostBuilderContext context, IConfigurationBuilder config)
        {
            var ternantID = "b6f5d995-6c61-4f47-8dec-3aafed7ab139";
            var appID = "e6396d12-c6ff-47f6-8858-ae3d3f24a133";
            var appSec = "6PZ7Q~xxxgLg1h5S0QfHAJIeJjGdaPZNi3N8R";

            var builderConfig = config.Build();
            var keyVaultName = builderConfig[$"AppSettings:KeyVaultName"];
            BaseUri = $"https://{keyVaultName}.vault.azure.net";
            var secretClient = new SecretClient(
                new Uri(BaseUri),
                // new DefaultAzureCredential()
                new ClientSecretCredential(ternantID, appID, appSec)
            );
            config.AddAzureKeyVault(secretClient, new KeyVaultSecretManager());
        }

        private static Dictionary<string, string> SecretsCache = new Dictionary<string, string>();

        public async static Task<string> GetCachedSecret(string secrectName)
        {
            if (!SecretsCache.ContainsKey(secrectName)) {
                var secretBundle = await KeyVaultClient.GetSecretAsync($"{BaseUri}/secrets/{secrectName}").ConfigureAwait(false);
                SecretsCache.Add(secrectName, secretBundle.Value);
            }
            return SecretsCache.ContainsKey(secrectName) ? SecretsCache[secrectName] : string.Empty;
        }
    }

    public static class GetSecret
    {
        public static async Task<string> PlayTogetherDbConnectionString() => await KeyVaultServices.GetCachedSecret($"{KeyVaultKeys.PlayTogetherDbConnectionString}");
        public static async Task<string> StorageAccountSecret() => await KeyVaultServices.GetCachedSecret($"{KeyVaultKeys.StorageAccount}");
    }
}