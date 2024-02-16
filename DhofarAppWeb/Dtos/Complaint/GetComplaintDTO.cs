using DhofarAppWeb.Dtos.ComplaintFiles;
using DhofarAppWeb.Model;

namespace DhofarAppWeb.Dtos.Complaint
{
    public class GetComplaintDTO
    {

        public string State { get; set; }
        public int DepartmentTypeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public DateTime Time { get; set; }
        public List<GetComplaintFilesDTO> Files { get; set; } 



    }
}
