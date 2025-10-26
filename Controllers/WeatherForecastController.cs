using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriviaApp.Contexts;
using TriviaApp.Models;

namespace TriviaApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        //private static readonly string[] Summaries = new[]
        //{
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly TriviaFetchService _triviaFetchService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly TriviaContext _triviaQuestionsContext;
        Random random = new Random();
        

        protected ICollection<TriviaQuestion> GenerateQuestions(MovieData movie)
        {

            string yearFromTitle = $"What year was {movie.title} released?";
            TriviaQuestion question = new TriviaQuestion();
            question.CorrectAnswer = movie.release_date.Substring(0, 4);
            question.QuestionTopic = movie.title;
            ICollection<TriviaQuestion> questions = new List<TriviaQuestion>();

            string[] wrongAnswers = new string[3];
            for (int i = 0; i < wrongAnswers.Length; i++)
            {
                string answer = random.Next(1940, DateTime.Now.Year).ToString();
                while (answer == question.CorrectAnswer)
                    answer = random.Next(1940, DateTime.Now.Year).ToString();

                wrongAnswers[i] = answer;

            }
            question.WrongAnswers = wrongAnswers;
            question.QuestionDescription = yearFromTitle;

            questions.Add(question);






            return questions;

        }
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration configuration , IHttpClientFactory httpClientFactory, TriviaContext triviaQuetionsContext)
        {
            _logger = logger;
            _configuration = configuration;

            _httpClientFactory = httpClientFactory;
            _triviaQuestionsContext= triviaQuetionsContext; 
            //IHttpClientFactory httpClientFactory= new IHttpClientFactory() ;
            //IConfiguration configuration = new IConfiguration();
            this._triviaFetchService= new TriviaFetchService(_httpClientFactory, _configuration);
        }

        [HttpGet(Name = "GenerateQuestions")]
        public async Task<IEnumerable<MovieData>> Get()
        {
            var topRatedMovies = await _triviaFetchService.GetMovieDetails<TopRatedMovies>();
            List<MovieData>? movies = new List<MovieData>();
            if (topRatedMovies != null)
            {
                movies = topRatedMovies.results.ToList();
                for (int i = 0; i < movies.Count; i++) {
                    List<TriviaQuestion> questions = (List<TriviaQuestion>)GenerateQuestions(movies[i] );


                    for (int j = 0; j < questions.Count; j++)
                    {
                        var question = questions[j];
                        _triviaQuestionsContext.triviaQuestions.Add(questions[j]);
                        Console.WriteLine( _triviaQuestionsContext.triviaQuestions.Find(question.Id).QuestionDescription);
                    }
                }
            }
            return movies;

        }
    }
}
