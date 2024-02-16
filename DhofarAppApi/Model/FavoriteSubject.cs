using System.ComponentModel.DataAnnotations;

namespace DhofarAppApi.Model
{
    public class FavoriteSubject
    {
        public int SubjectId { get; set; }

        [MaxLength(450)]
        public string UserId { get; set; }

        public Subject? Subject { get; set; }

        public User? User { get; set; }

    }
}
