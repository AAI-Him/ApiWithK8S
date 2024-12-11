using ApiWithK8S.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ApiWithK8S.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ISAPClientFactory _sapClientFactory;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            ISAPClientFactory sapClientFactory)
        {
            _logger = logger;
            _sapClientFactory = sapClientFactory;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                //Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                Summary = _sapClientFactory.AppHost
            })
            .ToArray();
        }
    }
}
