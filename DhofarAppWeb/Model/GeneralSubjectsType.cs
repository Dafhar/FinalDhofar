namespace DhofarAppWeb.Model
{
    public class GeneralSubjectsType
    {
        public int Id { get; set; }

        //public string Title { get; set; }

        public string Name_Ar { get; set; }
        public string Name_En { get; set; }

        public List<Subject> Subjects { get; set; }

       
    }
}
