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
            var ternantID = "04994528-d4e3-474f-a1e0-a4eb2ba9ef7e";
            var appID = "6e6d7fbe-b23f-42f9-a68c-457f47cce37f";
            var appSec = "GMP7Q~4C25Anfs8XXNYhK9z1-meuttPC~errY";

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