namespace DhofarAppApi.Model
{
    public class SubjectType
    {
        public int Id { get; set; }

        public required string Name_Ar { get; set; }

        public required string Name_En { get; set; }

        public List<SubjectTypeSubject> SubjectTypeSubjects { get; set; }


    }
}
