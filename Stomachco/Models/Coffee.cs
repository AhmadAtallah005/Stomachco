

using Stomachco.Models.SharedProp;
using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models
{
    public class Coffee:CommonProp
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
        public string? CoffeeImage { get; set; }


    }
}
