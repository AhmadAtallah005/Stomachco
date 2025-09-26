using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models.ViewModel
{

    public class EditRoleViewModel
    {

        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        public string? RoleID { get; set; }
        [Required(ErrorMessage = "Enter Role Name")]
        public string? RoleName { get; set; }

        public List<string> Users { get; set; }


    }
}
