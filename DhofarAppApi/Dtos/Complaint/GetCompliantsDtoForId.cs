using DhofarAppApi.Dtos.ComplaintFiles;

namespace DhofarAppApi.Dtos.Complaint
{
    public class GetCompliantsDtoForId
    {
       
            public int Id { get; set; }

            public string State { get; set; }
            public string DepartmentTypeName { get; set; }
            public string CategoryName { get; set; }
            public string? SubCategoryName { get; set; }

            public string Title { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
            public string Location { get; set; }
            public DateTime Time { get; set; }
            public List<GetComplaintFilesDTO> Files { get; set; }
        

    }
}
