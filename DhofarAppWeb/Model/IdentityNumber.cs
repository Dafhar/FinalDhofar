using System.ComponentModel.DataAnnotations;

namespace DhofarAppWeb.Model
{
    public class IdentityNumber
    {
        [Key]
        public int Id { get; set; }

        public int Value { get; set; }
    }
}
