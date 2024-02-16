using System.ComponentModel.DataAnnotations;

namespace DhofarAppApi.Dtos.User
{
    public class UpdatePasswordDTO
    {
         [Required]

        public string OldPassword { get; set; }
        [Required]

        public string NewPassword { get; set; }

    }
}
