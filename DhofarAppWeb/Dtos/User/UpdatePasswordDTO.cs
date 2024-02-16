using System.ComponentModel.DataAnnotations;

namespace DhofarAppWeb.Dtos.User
{
    public class UpdatePasswordDTO
    {
         [Required]

        public string OldPassword { get; set; }
        [Required]

        public string NewPassword { get; set; }

    }
}
