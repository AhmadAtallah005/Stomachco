using Stomachco.Models.SharedProp;
using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models.ViewModel
{
    public class SuperMarketViewModel:CommonProp
    {
        public Guid SuperMarketId { get; set; }
        [Display(Name = "SuperMarket Name")]
        [Required]
        public string? SMName { get; set; }

        [Required]
        public IFormFile? SMImage { get; set; }
        public string? Image { get; set; }

    }
}
