using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models.ViewModel
{
    public class FeedbackViewModel
    {
        [Required]
        [StringLength(1000)]
        public string? Message { get; set; }

    }
}
