using DhofarAppApi.Dtos.ComplaintFiles;
using DhofarAppApi.Model;

namespace DhofarAppApi.Dtos.Complaint
{
    public class GetComplaintDTO
    {
        public int Id { get; set; }
        public string State { get; set; }
        public int DepartmentTypeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
        public DateTime Time { get; set; }
        public List<GetComplaintFilesDTO> Files { get; set; } 



    }
    public class GetMyComplintDTO
    {
        public int Id { get; set; } 

        public string Title { get; set; }

        public string Status { get; set; }

        public DateTime Time { get; set; }

        public List<GetComplaintFilesDTO> Files { get; set; }


    }
}
