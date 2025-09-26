using Stomachco.Models.SharedProp;
using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models.ViewModel
{
    public class SMFoodsViewModel:CommonProp
    {
        public int ID { get; set; }

        [Display(Name = "SuperMarket Item")]
        [Required]
        public string? SMItem { get; set; }

        [Required]

        public IFormFile? FoodImg { get; set; }
        public string? Img { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string? PriceUnit { get; set; }
    }
}
