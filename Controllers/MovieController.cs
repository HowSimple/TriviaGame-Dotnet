using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriviaApp.Contexts;
using TriviaApp.Models;

namespace TriviaApp.Controllers
{
    // Retrives movies from public API, and generates trivia questions

    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly TriviaFetchService _triviaFetchService;
        private readonly TriviaContext _context;
        private Random random = new Random();

        public MovieController(TriviaContext context, TriviaFetchService triviaFetchService)
        {
            _context = context;
            _triviaFetchService = triviaFetchService;
        }

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

        // GET: api/Movie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieData>>> Getmovies()
        {
            return await _context.movies.ToListAsync();
        }

        // GET: api/Movie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieData>> GetMovieData(int id)
        {
            var movieData = await _context.movies.FindAsync(id);
            if (movieData == null)
            {
                return NotFound();
            }

            return movieData;
        }

        [HttpPatch]
        public async Task<IEnumerable<MovieData>> Generate()
        {
            var topRatedMovies = await _triviaFetchService.GetMovieDetails<TopRatedMovies>();
            List<MovieData>? movies = new List<MovieData>();
            if (topRatedMovies != null)
            {
                movies = topRatedMovies.results.ToList();
                for (int i = 0; i < movies.Count; i++)
                {
                    _context.movies.Add(movies[i]);

                    List<TriviaQuestion> questions =
                        (List<TriviaQuestion>)GenerateQuestions(movies[i]);

                    for (int j = 0; j < questions.Count; j++)
                    {
                        var question = questions[j];
                        _context.triviaQuestions.Add(questions[j]);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return movies;
        }
        // PUT: api/Movie/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovieData(int id, MovieData movieData)
        {
            if (id != movieData.id)
            {
                return BadRequest();
            }

            _context.Entry(movieData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movie
        [HttpPost]
        public async Task<ActionResult<MovieData>> PostMovieData(MovieData movieData)
        {
            _context.movies.Add(movieData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovieData", new { id = movieData.id }, movieData);
        }

        // DELETE: api/Movie/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieData(int id)
        {
            var movieData = await _context.movies.FindAsync(id);
            if (movieData == null)
            {
                return NotFound();
            }

            _context.movies.Remove(movieData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieDataExists(int id)
        {
            return _context.movies.Any(e => e.id == id);
        }
    }
}
