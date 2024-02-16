using System.ComponentModel.DataAnnotations;

namespace DhofarAppWeb.Dtos.User
{
    public class LoginInputDTO
    {
        [Required]

        public string PhoneNumber { get; set; }
        [Required]

        public string Password { get; set; }
    }
}
