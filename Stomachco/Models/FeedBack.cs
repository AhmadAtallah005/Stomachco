namespace Stomachco.Models
{
    public class FeedBack
    {
        public int Id { get; set; }

        public string? UserId { get; set; }   // مفتاح خارجي على AspNetUsers
        public string? Email { get; set; }    // إيميل اليوزر
        public string? Message { get; set; }  // الملاحظة


    }
}
