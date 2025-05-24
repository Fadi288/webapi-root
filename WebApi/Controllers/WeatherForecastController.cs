using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("fady")]
        public IActionResult GetFady()
        {
            return Ok("Fady Samy");
        }

        [HttpGet]
        [Route("fadi")]
        public IActionResult GetFadi()
        {
            return Ok("Fadi Sami");
        }

        [HttpGet]
        [Route("fodfod")]
        public IActionResult GetFodfod()
        {
            return Ok("Fod Fod");
        }
        [HttpGet]
        [Route("davdav")]
        public IActionResult GetDavdav()
        {
            return Ok("Davdav");
        }
        [HttpGet]
        [Route("davdavo")]
        public IActionResult GetDavdava()
        {
            return Ok("Davdavo");
        }


    }
}
