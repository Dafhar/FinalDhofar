using System.ComponentModel.DataAnnotations;

namespace DhofarAppWeb.Dtos.User
{
    public class LoginInputDTO
    {
        [Required]

        public string LoginPhoneNumber { get; set; }
        [Required]

        public string LoginPassword { get; set; }
    }
}
