using System.ComponentModel.DataAnnotations.Schema;

namespace DhofarAppWeb.Dtos.User
{
    public class UserDTO
    {
        public string Id { get; set; }

        public int IdentityNumber { get; set; }

        public string UserName { get; set; }

        public string Token { get; set; }

        public IList<string> Roles { get; set; }
    }
}
