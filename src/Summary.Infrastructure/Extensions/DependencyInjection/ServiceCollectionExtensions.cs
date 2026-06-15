using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Summary.Core.Interfaces;
using Summary.Infrastructure.Callers;
using Summary.Infrastructure.Configurations;

namespace Summary.Infrastructure.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AzureLanguageServiceConfigurations>()
            .Bind(configuration.GetSection(AzureLanguageServiceConfigurations.SectionName))
            .Validate(config => !string.IsNullOrWhiteSpace(config.Endpoint), "AzureLanguageService Endpoint must be configured.")
            .Validate(config => !string.IsNullOrWhiteSpace(config.ApiKey), "AzureLanguageService ApiKey must be configured.")
            .ValidateOnStart();

        services.AddScoped<ILanguageServiceCaller, AzureLanguageServiceCaller>();

        return services;
    }
}
