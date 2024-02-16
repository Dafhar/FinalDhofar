using DhofarAppApi.Dtos.ComplaintFiles;
using DhofarAppApi.Model;

namespace DhofarAppApi.Dtos.Complaint
{
    public class PostComplaintDTO
    {
        public string State { get; set; }
        public int DepartmentTypeId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public List<string> files { get; set; }
        

    }

}
