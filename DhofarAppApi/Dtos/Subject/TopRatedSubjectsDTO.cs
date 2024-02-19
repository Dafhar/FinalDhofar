using DhofarAppApi.Dtos.SubjectFiles;

namespace DhofarAppApi.Dtos.Subject
{
    public class TopRatedSubjectsDTO
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string UserImageUrl { get; set; }

        public DateTime CreatedTime { get; set; }

        public string PrimarySubject { get; set; }

        public string Description { get; set; }

        public GetSubjectFilesDTO File { get; set; }

    }
}
