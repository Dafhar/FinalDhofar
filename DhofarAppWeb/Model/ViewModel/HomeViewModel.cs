namespace DhofarAppWeb.Model.ViewModel
{
    public class HomeViewModel
    {
        public List<GeneralSubjectsType> GeneralSubjectsTypes { get; set; }

        public int Visitors { get; set; }

        public int Complaints { get; set; }

        public List<User> Users { get; set; }
            
        public Subject Subjects { get; set; }

        public int SubjectSuggests { get; set; }

        public List<SubjectType> SubjectTypes { get; set; }
    }
}
