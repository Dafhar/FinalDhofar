namespace DhofarAppApi.Model
{
    public class Category
    {
        public int Id { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
        public List<SubCategory> subcategories { get; set;}
        public List<Complaint>Complaints { get; set; }
    }
}
