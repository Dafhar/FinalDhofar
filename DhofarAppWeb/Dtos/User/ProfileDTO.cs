using DhofarAppWeb.Model;

namespace DhofarAppWeb.Dtos.User
{
    public class ProfileDTO
    {
        public string? UserName { get; set; }

        public string ?FullName { get; set; }

        public string? Gender { get; set; }
        public string? Country { get; set; }
        public string? CountryCode { get; set; }


        public string? PhoneNumber { get; set; }

        public string? Description { get; set; }

        public string? File { get; set; }

        public string? Color { get; set; }
    }
}
