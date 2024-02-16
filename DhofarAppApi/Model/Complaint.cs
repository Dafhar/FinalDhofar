using System.ComponentModel.DataAnnotations.Schema;

namespace DhofarAppApi.Model
{
    public class Complaint
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string State { get; set; }

        public int DepartmentTypeId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Status_En { get; set; }
        public string Status_Ar { get; set; }

        public string Location { get; set; }

        public bool IsAccepted { get; set; } = false;
        public bool MySpecialist { get; set; } = true;

        public DateTime Time { get; set; }

        public List<ComplaintsFile>? Files { get; set; } 

        public User User { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

       public Category? Category { get; set; }

        public DepartmentType DepartmentType { get; set; }

    }
}
