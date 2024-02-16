using System.ComponentModel.DataAnnotations;

namespace DhofarAppApi.Dtos.User
{
    public class LoginInputDTO
    {
        [Required]

        public string PhoneNumber { get; set; }
        [Required]

        public string Password { get; set; }
    }
}
