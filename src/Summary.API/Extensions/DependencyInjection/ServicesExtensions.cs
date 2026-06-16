using Microsoft.AspNetCore.Authentication;
using Summary.API.Authentication;
using Summary.API.Configurations;

namespace Summary.API.Extensions.DependencyInjection;

public static class ServicesExtensions
{
    public static IServiceCollection AddApiKeyAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<ApiKeyConfigurations>()
            .Bind(configuration.GetSection(ApiKeyConfigurations.SectionName))
            .Validate(config => !string.IsNullOrWhiteSpace(config.Key), "ApiKey must be configured.")
            .ValidateOnStart();

        services.AddAuthentication(ApiKeyAuthenticationOptions.SchemeName)
            .AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(
                ApiKeyAuthenticationOptions.SchemeName, _ => { });

        services.AddAuthorization();

        return services;
    }
}
