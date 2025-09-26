using Stomachco.Models.SharedProp;
using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models
{
    public class CoffeeDrinks :CommonProp
    {
        public int ID { get; set; }

        [Display(Name = "Drink Name")]
        [Required]
        public string? DrinkName { get; set; }
        [Display(Name = "Drink Description")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string? DrinkDescription { get; set; }
        [Required]

        public string? DrinkImg { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string? PriceUnit { get; set; }
    }
}
