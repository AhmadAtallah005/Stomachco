using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]


        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Not Match Passwords")]

        public string? ConfirnPassword { get; set; }
    }
}
