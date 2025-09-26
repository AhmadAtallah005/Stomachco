using Stomachco.Models.SharedProp;
using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models
{
    public class Restaurant:CommonProp
    {
        public Guid RestaurantId { get; set; }
        [Display(Name = "Restaurant Name")]
        [Required]
        public string? RestName { get; set; }
        [Display(Name = "Restaurant Description")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string? RestDesciption { get; set; }
     
        [Required]
        public string? RestImage { get; set; }



    }
}
