namespace DhofarAppWeb.Model
{
    public class SubjectType
    {
        public int Id { get; set; }
        public required string Title_Ar { get; set; }
        public required string Title_En { get; set; }

        public List<Subject> Subjects { get; set; }
    }
}
