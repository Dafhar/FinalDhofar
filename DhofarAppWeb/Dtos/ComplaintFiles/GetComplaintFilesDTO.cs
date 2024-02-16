namespace DhofarAppWeb.Dtos.ComplaintFiles
{
    public class GetComplaintFilesDTO
    {
        public int Id { get; set; }
        public int ComplaintId { get; set; }

        public string? FilePaths { get; set; }

    }
}
