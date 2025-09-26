using System.ComponentModel.DataAnnotations;

namespace Stomachco.Models.SharedProp
{
    public class CommonProp
    {

        [Display(Name = "Creation Date")]
        [DataType(DataType.DateTime)]
        public DateTime? CreationDate { get; set; } = DateTime.Now;
        [Display(Name ="Is Published")]
        public bool isPublished { get; set; }
        [Display(Name = "Is Deleted")]
        public bool isDeleted { get; set; }

        public int UserId { get; set; }




    }
}
