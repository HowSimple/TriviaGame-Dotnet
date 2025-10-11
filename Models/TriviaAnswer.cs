using System.ComponentModel.DataAnnotations;

namespace TriviaApp.Models
{
    public class TriviaAnswer
    {
        [Key]
        public int Id { get; set; }
        public string? Answer { get; set; }
        public Boolean isCorrect {  get; set; }



    }
}
