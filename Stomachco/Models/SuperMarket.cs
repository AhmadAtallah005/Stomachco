using Stomachco.Models.SharedProp;
using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models
{
    public class SuperMarket:CommonProp
    {
        public Guid SuperMarketId { get; set; }
        [Display(Name = "SuperMarket Name")]
        [Required]
        public string? SMName { get; set; }

        [Required]
        public string? SMImage { get; set; }
    }
}
