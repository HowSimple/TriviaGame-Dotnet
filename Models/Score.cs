using System.ComponentModel.DataAnnotations;

namespace TriviaApp.Models
{
    public class Score : EntityBase
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        
        [Required]
        [MaxLength(256)]
        public string UserId { get; set; } = string.Empty;


        [MaxLength(256)]
        public string? UserName { get; set; }

        [Required]
        public int Value { get; set; }
    }
}