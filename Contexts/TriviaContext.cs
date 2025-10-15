using Microsoft.EntityFrameworkCore;
using TriviaApp.Models;

namespace TriviaApp.Contexts
{
    public class TriviaContext :DbContext
    {
        public TriviaContext(DbContextOptions<TriviaContext> options) : base(options) 
        {
        //    object value = Database.SetInitializer(TriviaContext)(new CreateDatabaseIfNot);
        }
        public DbSet<TriviaQuestion> triviaQuestions { get; set; } = null;
        public DbSet<TriviaAnswer> triviaAnswers { get; set; } = null;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            //modelBuilder.Entity<TriviaAnswer>().HasData(new Tri)
            //base.OnModelCreating(modelBuilder);
        }


    }

}
