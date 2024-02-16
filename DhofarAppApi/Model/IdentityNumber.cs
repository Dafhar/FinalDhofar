using System.ComponentModel.DataAnnotations;

namespace DhofarAppApi.Model
{
    public class IdentityNumber
    {
        [Key]
        public int Id { get; set; }

        public int Value { get; set; }
    }
}
