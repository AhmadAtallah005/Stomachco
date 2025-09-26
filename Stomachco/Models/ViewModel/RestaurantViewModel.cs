using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.FileProviders;
using Stomachco.Models.SharedProp;

namespace Stomachco.Models.ViewModel
{
    public class RestaurantViewModel: CommonProp
    {
        public Guid RestaurantId { get; set; }
        [Display(Name = "Restaurant Name")]
        [Required]
        public string? RestName { get; set; }
        [Display(Name = "Restaurant Description")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string? RestDesciption { get; set; }

        public IFormFile? RestImage { get; set; }
        public string? Image { get; set; }
    }
}
