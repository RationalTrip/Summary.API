using Azure.Monitor.OpenTelemetry.AspNetCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Summary.API.Configurations;
using Summary.API.Telemetry;

namespace Summary.API.Extensions.DependencyInjection;

public static class OpenTelemetryConfigurationExtensions
{
    public static ILoggingBuilder ConfigureCustomLogging(this ILoggingBuilder loggingBuilder)
        => loggingBuilder.AddOpenTelemetry(options =>
            {
                options.IncludeFormattedMessage = true;
                options.ParseStateValues = true;
                options.IncludeScopes = true;
            });

    public static IServiceCollection AddCustomOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind and validate configuration immediately
        var openTelemetryConfig = configuration.GetSection(OpenTelemetryConfiguration.SectionName).Get<OpenTelemetryConfiguration>();

        if (openTelemetryConfig is null)
            throw new InvalidOperationException($"Configuration section '{OpenTelemetryConfiguration.SectionName}' is missing or invalid.");

        // Register configuration with validation
        services.AddOptions<OpenTelemetryConfiguration>()
            .Bind(configuration.GetSection(OpenTelemetryConfiguration.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(
                    serviceName: openTelemetryConfig.ServiceName,
                    serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0");
            })
            .WithTracing(tracer =>
            {
                tracer.AddAspNetCoreInstrumentation(options =>
                {
                    options.RecordException = true;
                });

                tracer.AddHttpClientInstrumentation()
                    .AddSource("Azure.*");
            })
            .WithMetrics(metrics =>
            {
                metrics.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation();
            })
            .UseAzureMonitor(options =>
            {
                options.ConnectionString = openTelemetryConfig.ConnectionString;
            });

        if (openTelemetryConfig.SamlingRatio <= 0 || openTelemetryConfig.SamlingRatio > 1)
            throw new InvalidOperationException($"Configuration section '{OpenTelemetryConfiguration.SectionName}' '{nameof(OpenTelemetryConfiguration.SamlingRatio)}' configuration error. " +
                $"Expected value should be in range (0, 1]. Actual value {openTelemetryConfig.SamlingRatio}");

        // Added separately because 'UseAzureMonitor' can override Sampler defined within 'WithTracing'
        services.ConfigureOpenTelemetryTracerProvider((sp, tracer) =>
            tracer.SetSampler(new RequestAllowedSampler(openTelemetryConfig.SamlingRatio)));

        return services;
    }
}
