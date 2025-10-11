using Microsoft.EntityFrameworkCore;
using TriviaApp.Models;

namespace TriviaApp.Contexts
{
    public class TriviaContext :DbContext
    {
        public TriviaContext(DbContextOptions<TriviaContext> options) : base(options) 
        {

        }
        public DbSet<TriviaQuestion> triviaQuestions { get; set; } = null;
        public DbSet<TriviaAnswer> triviaAnswers { get; set; } = null;






    }

}
