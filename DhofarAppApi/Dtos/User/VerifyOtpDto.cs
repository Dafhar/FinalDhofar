using System.ComponentModel.DataAnnotations;

namespace DhofarAppApi.Dtos.User
{
    public class VerifyOtpDto
    {
        [Required]

        public string phoneNumber { get; set; }

        [Required]

        public string otpCode { get; set; }
        [Required]

        public string createdAt { get; set; }
        [Required]

        public string hashString { get; set; }
    }
}
