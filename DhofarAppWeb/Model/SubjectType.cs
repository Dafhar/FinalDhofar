namespace DhofarAppWeb.Model
{
    public class SubjectType
    {
        public int Id { get; set; }

        public string TitleValue { get; set; }

        public string Name_Ar { get; set; }
        public string Name_En { get; set; }

        public List<Subject> Subjects { get; set; }

       
    }
}
