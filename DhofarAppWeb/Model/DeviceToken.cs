using System.ComponentModel.DataAnnotations;

namespace DhofarAppWeb.Model
{
    public class DeviceToken
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Token { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

    }
}
