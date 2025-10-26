using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TriviaApp.Models;

namespace TriviaApp.Persistance.Configurations
{
  

    public class TriviaQuestionConfiguration : IEntityTypeConfiguration<TriviaQuestion>
    {
        public void Configure(EntityTypeBuilder<TriviaQuestion> builder)
        {
            // Define table name
            builder.ToTable("TriviaQuestion");

            // Set primary key
            builder.HasKey(m => m.Id);

            // Configure properties
            builder.Property(m => m.QuestionDescription)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(m => m.WrongAnswers)
                   .IsRequired()
                   .HasMaxLength(5);

            builder.Property(m => m.CorrectAnswer)
                   .IsRequired();

            // Configure Created and LastModified properties to be handled as immutable and modifiable timestamps
            builder.Property(m => m.Created)
                   .IsRequired()
                   .ValueGeneratedOnAdd();

            builder.Property(m => m.LastModified)
                   .IsRequired()
                   .ValueGeneratedOnUpdate();

            // Optional: Add indexes for better query performance
            builder.HasIndex(m => m.QuestionTopic);

        }
    }
}
