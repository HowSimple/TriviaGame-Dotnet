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
        public DbSet<Score> scores { get; set; }
        //public DbSet<TriviaAnswer> triviaAnswers { get; set; }

        public DbSet<MovieData> movies{ get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.HasDefaultSchema("Trivia");
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(TriviaContext).Assembly);
            //modelBuilder.Entity<TriviaAnswer>().HasData(new Tri)
            base.OnModelCreating(modelBuilder);
              modelBuilder.Entity<Score>(entity =>
            {
                entity.ToTable("Scores");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired().HasMaxLength(256);
                entity.Property(e => e.UserName).HasMaxLength(256);
                entity.Property(e => e.Value).IsRequired();
                entity.HasIndex(e => e.UserId); // fast lookups by user
            });
        }


    }

}
