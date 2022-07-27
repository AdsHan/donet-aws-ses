using AmazonSimpleEmail.API.Utilis;
using Microsoft.Extensions.Options;

namespace AmazonSimpleEmail.API.Configuration;

public static class SettingsConfig
{
    public static IServiceCollection AddSettingsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var tokenSettings = new TokenSettings();
        new ConfigureFromConfigurationOptions<TokenSettings>(configuration.GetSection("TokenSettings")).Configure(tokenSettings);

        var emailSettings = new EmailSettings();
        new ConfigureFromConfigurationOptions<EmailSettings>(configuration.GetSection("EmailSettings")).Configure(emailSettings);

        services.AddSingleton(tokenSettings);
        services.AddSingleton(emailSettings);

        return services;
    }

}
