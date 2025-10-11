using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TriviaApp.Models
{

    public class TriviaQuestion
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string QuestionHeader { get; set; }

        public string QuestionDescription { get; set; }
        public ICollection<TriviaAnswer> Answers { get;  } = new List<TriviaAnswer>();

        public TriviaAnswer correctAnswer { get; set; } 

        public string QuestionCategory {  get; set; }




    }
}
