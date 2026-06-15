using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Summary.Application.Configurations;

namespace Summary.Application.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure and validate SummarizeConfigurations
        services.AddOptions<SummarizeConfigurations>()
            .Bind(configuration.GetSection(SummarizeConfigurations.SectionName))
            .Validate(config => config.MaxInputLength > 0, "MaxInputLength must be greater than 0.")
            .ValidateOnStart();

        // Register FluentValidation validators
        services.AddValidatorsFromAssemblyContaining<ApplicationProject>();

        return services;
    }
}
