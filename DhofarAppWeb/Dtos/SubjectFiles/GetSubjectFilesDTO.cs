namespace DhofarAppWeb.Dtos.SubjectFiles
{
    public class GetSubjectFilesDTO
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }

        public string? FilePaths { get; set; }
    }
}
