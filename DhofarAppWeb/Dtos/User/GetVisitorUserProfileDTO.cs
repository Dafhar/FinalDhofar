using DhofarAppWeb.Dtos.Subject;

namespace DhofarAppWeb.Dtos.User
{
    public class GetVisitorUserProfileDTO
    {
        public int SubjectCounter { get; set; }

        public int CommentCounter { get; set; }

        public double Rate { get; set; }

        public string FullName { get; set; }

        public string? ImageUrl { get; set; }

        public string Description { get; set; }

        public List<GetSubjectForProfileDTO> Subjects { get; set; }
    }
}
