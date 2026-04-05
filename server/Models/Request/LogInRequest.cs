using System.ComponentModel.DataAnnotations;

namespace IMDB.Models.Request
{
    public class LogInRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
