namespace DhofarAppApi.Model
{
    public class RatingSubject
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }

        public string UserId { get; set; }

        public bool IsLike { get; set; } = false;

        public bool IsDisLike { get; set; } = false;

        public Subject Subject { get; set; }

        public User User { get; set; }
    }
}
