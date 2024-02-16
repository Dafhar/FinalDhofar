using DhofarAppWeb.Dtos.ComplaintFiles;

namespace DhofarAppWeb.Dtos.Complaint
{
    public class GetAllComplaintAdmin
    {

        public string State { get; set; }
        public int DepartmentTypeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status_En { get; set; }
        public string Status_Ar { get; set; }
        public string Location { get; set; }
        public DateTime Time { get; set; }
        public List<GetComplaintFilesDTO> Files { get; set; }
    }
}
