namespace DhofarAppWeb.Model
{
    public class SubjectTypeSubject
    {
        public int SubjectId { get; set; }

        public int SubjectTypeId { get; set; }

        public Subject Subject { get; set; }

        public SubjectType SubjectType { get; set; }
    }
}
        