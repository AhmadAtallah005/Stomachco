using Stomachco.Models.SharedProp;
using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models
{
    public class RestFood:CommonProp
    {
        public int ID { get; set; }

        [Display(Name = "Food Name")]
        [Required]
        public string? FoodName { get; set; }
        [Display(Name = "Food Description")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string? FoodDescription { get; set; }
        [Required]

        public string? FoodImg { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string? PriceUnit { get; set; }
    }
}
