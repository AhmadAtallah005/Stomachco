using Stomachco.Models.SharedProp;
using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models.ViewModel
{
    public class RestFoodsViewModel:CommonProp
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

        public IFormFile? FoodImg { get; set; }
        public string? Img { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string? PriceUnit { get; set; }
    }
}
