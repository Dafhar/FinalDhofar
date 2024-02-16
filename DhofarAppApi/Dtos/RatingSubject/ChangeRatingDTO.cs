namespace DhofarAppApi.Dtos.RatingSubjectDTO
{
    public class ChangeRatingDTO
    {

        public int SubjectId { get; set; }

        public string UserId { get; set; }

        public bool IsLike { get; set; } = false;

        public bool IsDisLike { get; set; } = false;
    }
}
