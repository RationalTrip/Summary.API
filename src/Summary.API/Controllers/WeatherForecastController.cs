using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Summary.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        private static readonly ActivitySource ActivitySource = new("Summary.API");
        private static readonly Meter Meter = new("Summary.API");
        private static readonly Counter<int> ForecastRequestCounter = Meter.CreateCounter<int>("weather_forecast_requests", "requests", "Number of weather forecast requests");

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            // Create a custom activity (span) for detailed tracing
            using var activity = ActivitySource.StartActivity("GenerateWeatherForecast");

            // Log information
            _logger.LogInformation("Generating weather forecast for 5 days");

            // Increment custom metric
            ForecastRequestCounter.Add(1, new KeyValuePair<string, object?>("endpoint", "GetWeatherForecast"));

            // Add custom tags to the activity
            activity?.SetTag("forecast.days", 5);
            activity?.SetTag("forecast.location", "Default");

            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();

            _logger.LogInformation("Successfully generated {Count} weather forecasts", forecasts.Length);

            return forecasts;
        }
    }
}
