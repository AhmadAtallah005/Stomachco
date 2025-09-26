using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models.ViewModel
{

    public class CreateRoleViewModel
    {

        [Required]
        public string? RoleName { get; set; }
    }
}
