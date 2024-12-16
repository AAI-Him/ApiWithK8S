using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Reflection;

namespace ApiWithK8S.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly Lazy<WeatherForecast> _lazyObject = new Lazy<WeatherForecast>(new WeatherForecast
        {
            TemperatureC = 1,
            Summary = "Lazy initialized in ValuesController"
        });

        [HttpGet(Name = "GetValues")]
        public IEnumerable<WeatherForecast> Get()
        {
            if (_lazyObject.IsValueCreated)
                return new[] { _lazyObject.Value };
            else
                return Enumerable.Empty<WeatherForecast>();
        }
    }
}
