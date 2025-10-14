using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TriviaApp.Models;

namespace TriviaApp.Controllers
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
        private readonly TriviaFetchService _triviaFetchService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration , IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;

            _httpClientFactory = httpClientFactory;
            //IHttpClientFactory httpClientFactory= new IHttpClientFactory() ;
            //IConfiguration configuration = new IConfiguration();
            this._triviaFetchService= new TriviaFetchService(_httpClientFactory, _configuration);
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<MovieData>> Get()
        {
            ICollection<MovieData> movies = new List<MovieData>();
            var movieData = await _triviaFetchService.GetMovieDetails<MovieData>();
            if (movieData != null)
            {
                movies.Add(movieData);
            }

            Console.WriteLine(movies.First().original_title);
            return movies;


            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();
        }
    }
}
