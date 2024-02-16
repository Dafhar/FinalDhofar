namespace DhofarAppApi.Model
{
    public class MainTopic
    {
        public int Id { get; set; }
        public required string Title_Ar { get; set; }
        public required string Title_En { get; set; }

        public List<SubjectType> subjectTypes { get; set; }
    }
}
