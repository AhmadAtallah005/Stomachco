using Stomachco.Models.SharedProp;
using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models.ViewModel
{
    public class CoffeeDrinksViewModel : CommonProp
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

        public IFormFile? DrinkImg { get; set; }
        public string? Img { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string? PriceUnit { get; set; }
    }
}
