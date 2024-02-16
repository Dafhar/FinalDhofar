namespace DhofarAppApi.Model
{
    public class SubCategory
    {
        public int Id { get; set; }

        public string  Name_En { get; set; }
        public string  Name_Ar { get; set; }


        public int CategoryId { get; set; }
        public Category category { get; set; }
    }
}
