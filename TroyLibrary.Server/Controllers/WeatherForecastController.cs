using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TroyLibrary.Common.Book;
using TroyLibrary.Service;
using TroyLibrary.Service.Interfaces;

namespace TroyLibrary.Server.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IBookService _bookService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IBookService bookService)
        {
            _logger = logger;
            _bookService = bookService;
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

        [HttpGet("Featured")]
        public GetBooksResponse GetFeaturedBooks()
        {
            return new GetBooksResponse
            {
                Books = this._bookService.GetFeaturedBooks(),
            };
        }
    }
}
