namespace TriviaApp.Models
{
    public class Account
    {
        public string EmailAddress { get; set; }

        public string Name { get; set; }
        public ICollection<Score> Scores { get; set; }
    }
}
