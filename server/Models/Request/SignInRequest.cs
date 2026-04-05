using System.ComponentModel.DataAnnotations;

namespace IMDB.Models.Request
{
    public class SignInRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Compare("ConfirmPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }
    }
}
