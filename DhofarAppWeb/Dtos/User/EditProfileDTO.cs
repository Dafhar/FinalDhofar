using DhofarAppWeb.Model;

namespace DhofarAppWeb.Dtos.User
{
    public class EditProfileDTO
    {
        public string? UserName { get; set; }

        public string ?FullName { get; set; }

        public string? Gender { get; set; }
        public string? Country { get; set; }
        public string? ImgrUrl { get; set; }

        
        public string? PhoneNumber { get; set; }

        public string? Description { get; set; }


        public int? ColorId { get; set; }
    }
}
