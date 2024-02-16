using DhofarAppWeb.Dtos.ComplaintFiles;

namespace DhofarAppWeb.Dtos.Complaint
{
    public class GetAllRejectedDTO
    {
        public string State { get; set; }
        public int DepartmentTypeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }

        public bool IsAccepted { get; set; }
        public DateTime Time { get; set; }
        public List<GetComplaintFilesDTO> Files { get; set; }
    }
}
