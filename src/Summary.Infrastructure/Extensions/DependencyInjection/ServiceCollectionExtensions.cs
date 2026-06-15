using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Azure;
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
            .Validate(config => config.MaxDocumentCharacterLength is > 0 and <= 5120, "AzureLanguageService MaxDocumentCharacterLength must be between 1 and 5120.")
            .Validate(config => config.MaxDegreeOfParallelism > 0, "AzureLanguageService MaxDegreeOfParallelism must be greater than 0.")
            .ValidateOnStart();

        services.AddAzureClients(builder =>
        {
            var config = configuration
                .GetSection(AzureLanguageServiceConfigurations.SectionName)
                .Get<AzureLanguageServiceConfigurations>()!;

            builder.AddTextAnalyticsClient(new Uri(config.Endpoint), new AzureKeyCredential(config.ApiKey));
        });

        services.AddScoped<ILanguageServiceCaller, AzureLanguageServiceCaller>();

        return services;
    }
}
