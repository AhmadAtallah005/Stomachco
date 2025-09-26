using Stomachco.Models.SharedProp;
using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models
{
    public class SMFood:CommonProp
    {
        public int ID { get; set; }

        [Display(Name = "SuperMarket Item")]
        [Required]
        public string? SMItem { get; set; }

        [Required]

        public string? FoodImg { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string? PriceUnit { get; set; }
    }
}
