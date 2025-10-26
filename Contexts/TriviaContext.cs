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
        public DbSet<TriviaQuestion> triviaQuestions { get; set; }
        public DbSet<TriviaAnswer> triviaAnswers { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.HasDefaultSchema("Trivia");
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(TriviaContext).Assembly);
            //modelBuilder.Entity<TriviaAnswer>().HasData(new Tri)
            base.OnModelCreating(modelBuilder);
        }


    }

}
