namespace DhofarAppWeb.Model
{
    public class DepartmentType
    {
        public int Id { get; set; }

        public required string Name_En { get; set; }
        public required string Name_Ar { get; set; }

        public string? UserId { get; set; }

        public List<Complaint> Complaints { get; set; }

        public List<DepartmentAdmin> DepartmentAdmins { get; set; }
    }
}
