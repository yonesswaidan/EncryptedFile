using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EncryptedFileApp.Services
{
    public class AzureKeyVaultService
    {
        private readonly SecretClient _secretClient;
        private readonly ILogger<AzureKeyVaultService> _logger;

        public AzureKeyVaultService(IConfiguration configuration, ILogger<AzureKeyVaultService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var keyVaultUrl = configuration["KeyVaultUrl"];
            if (string.IsNullOrEmpty(keyVaultUrl))
            {
                throw new ArgumentException("KeyVaultUrl must be configured in appsettings.json");
            }

            _logger.LogInformation("AzureKeyVaultService initialized with URL: {KeyVaultUrl}", keyVaultUrl);

            _secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
        }

        public async Task StoreKeyAsync(string fileId, byte[] key, byte[] iv)
        {
            string combined = Convert.ToBase64String(key) + ":" + Convert.ToBase64String(iv);

            _logger.LogInformation("Storing key and IV for fileId: {FileId} to Azure Key Vault", fileId);

            await _secretClient.SetSecretAsync(fileId, combined);

            _logger.LogInformation("Key and IV successfully stored for fileId: {FileId}", fileId);
        }

        public async Task<(byte[] Key, byte[] IV)?> GetKeyAsync(string fileId)
        {
            _logger.LogInformation("Retrieving key and IV for fileId: {FileId} from Azure Key Vault", fileId);

            try
            {
                var secret = await _secretClient.GetSecretAsync(fileId);
                var parts = secret.Value.Value.Split(':');

                if (parts.Length == 2)
                {
                    byte[] key = Convert.FromBase64String(parts[0]);
                    byte[] iv = Convert.FromBase64String(parts[1]);

                    _logger.LogInformation("Successfully retrieved key and IV for fileId: {FileId}", fileId);

                    return (key, iv);
                }
                else
                {
                    _logger.LogWarning("Invalid secret format for fileId: {FileId}", fileId);
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving key for fileId: {FileId}", fileId);
                return null;
            }
        }
    }
}
