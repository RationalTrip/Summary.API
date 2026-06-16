using Azure;
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
        services.AddOptions<TextAnalyticsClientConfigurations>()
            .Bind(configuration.GetSection(TextAnalyticsClientConfigurations.SectionName))
            .Validate(config => !string.IsNullOrWhiteSpace(config.Endpoint), "AzureLanguageService Endpoint must be configured.")
            .Validate(config => !string.IsNullOrWhiteSpace(config.ApiKey), "AzureLanguageService ApiKey must be configured.")
            .ValidateOnStart();

        services.AddOptions<AzureLanguageServiceConfigurations>()
            .Bind(configuration.GetSection(AzureLanguageServiceConfigurations.SectionName))
            .Validate(config => config.DocumentSizeLimit > 0, "DocumentSizeLimit must be greater than 0.")
            .Validate(config => config.DocumentPerBatchLimit > 0, "DocumentPerBatchLimit must be greater than 0.")
            .ValidateOnStart();

        services.AddAzureClients(builder =>
        {
            var config = configuration
                .GetSection(TextAnalyticsClientConfigurations.SectionName)
                .Get<TextAnalyticsClientConfigurations>()!;

            builder.AddTextAnalyticsClient(new Uri(config.Endpoint), new AzureKeyCredential(config.ApiKey));
        });

        services.AddScoped<ILanguageServiceCaller, AzureLanguageServiceCaller>();

        return services;
    }
}
