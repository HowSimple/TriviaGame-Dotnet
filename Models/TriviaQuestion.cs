using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TriviaApp.Models
{

    public class TriviaQuestion : EntityBase 
    {
        
    

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        //public string QuestionHeader { get; set; }
        [Required]
        public string QuestionDescription { get; set; }


        public string QuestionTopic { get; set; }
        [Required]
        public string[] WrongAnswers { get; set; }
        [Required]
        public string CorrectAnswer { get; set; } 

        //public string QuestionType {  get; set; }


        //public string[] GenerateQuestions(MovieData movie) {

        //    string yearFromTitle = $"What year was {movie.title} released?";
        //      correctAnswer = movie.release_date.Substring(0, 3);

        //    string[] wrongAnswers = new string[3];
        //    for (int i = 0; i < wrongAnswers.Length; i++) {
        //        string answer = random.Next(1940, DateTime.Now.Year).ToString();
        //        while(answer == movie.release_date)
        //        answer = random.Next(1940, DateTime.Now.Year).ToString();

        //        wrongAnswers[i] = answer;
        //    }
        //    return wrongAnswers;

        //}




    }
}
