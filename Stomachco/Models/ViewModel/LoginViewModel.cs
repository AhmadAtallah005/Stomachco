using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.Password)]

        public string? Password { get; set; }
    }
}
