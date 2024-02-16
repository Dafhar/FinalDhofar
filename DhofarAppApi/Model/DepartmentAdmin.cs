namespace DhofarAppApi.Model
{
    public class DepartmentAdmin
    {
        public int DepartmentTypeId { get; set; }

        public string UserId { get; set; }

        public DepartmentType DepartmentType { get; set; }

        public User User { get; set; }
    }
}
