using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace FeatureToggle.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
  private static readonly string[] Summaries = new[]
  {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

  private readonly ILogger<WeatherForecastController> _logger;
  private readonly IFeatureManager _featureManager;

  public WeatherForecastController(ILogger<WeatherForecastController> logger, IFeatureManager featureManager)
  {
    _logger = logger;
    _featureManager = featureManager;
  }

  [HttpGet(Name = "GetWeatherForecast")]
  [FeatureGate("WeatherForecast")]
  public async Task<IEnumerable<WeatherForecast>> Get()
  {
    bool summaryEnabled = await _featureManager.IsEnabledAsync("WeatherSummary");

    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
      Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
      TemperatureC = Random.Shared.Next(-20, 55),
      Summary = summaryEnabled ? Summaries[Random.Shared.Next(Summaries.Length)] : null
    }).ToArray();
  }
}
