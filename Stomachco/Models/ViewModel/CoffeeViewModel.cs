using Stomachco.Models.SharedProp;
using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models.ViewModel
{
    public class CoffeeViewModel:CommonProp
    {
        public Guid CoffeeId { get; set; }
        [Display(Name = "Coffe Name")]
        [Required]
        public string? CofeName { get; set; }
        [Display(Name = "Coffe Description")]
        [Required]
        [DataType(DataType.MultilineText)]

        public string? CofeDescription { get; set; }
        [Required]
        public IFormFile? CoffeeImage { get; set; }
        public string? Image { get; set; }
    }
}
