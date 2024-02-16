using System.ComponentModel.DataAnnotations;

namespace DhofarAppApi.Dtos.User
{
    public class RegisterEmployeeDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Fullname { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string CodeNumber { get; set; }

        [Required]
        public int DepatmentTypeId { get; set; }
    }
}
