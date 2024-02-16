using System.ComponentModel.DataAnnotations;

namespace DhofarAppWeb.Dtos.User
{
    public class LoginDTO
    {

        public string UserName { get; set; }


        [Required]
        [DataType(DataType.PhoneNumber)]

        
        public string PhoneNumber { get; set; }

       
        public string Token { get; set; }

        public IList<string> Roles { get; set; }

    }
}
