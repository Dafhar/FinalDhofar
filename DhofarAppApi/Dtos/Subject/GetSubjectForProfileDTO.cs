using DhofarAppApi.Dtos.SubjectType;

namespace DhofarAppApi.Dtos.Subject
{
    public class GetSubjectForProfileDTO
    {
        public int Id { get; set; }

        public string? SubjectTypeName { get; set; }

        public string? ImageUrl { get; set; }

        public string? PrimarySubject { get; set; }

        public DateTime CreatedTime { get; set; }

        public int CommentCounter { get; set; }

        public List<string> UserImages { get; set; } = new List<string>();



    }
}
