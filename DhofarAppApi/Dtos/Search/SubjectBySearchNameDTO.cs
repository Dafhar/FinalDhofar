using DhofarAppApi.Dtos.SubjectFiles;

namespace DhofarAppApi.Dtos.Search
{
    public class SubjectBySearchNameDTO
    {
        public int Id { get; set; }

        public List<string> SubjectTypeName { get; set; }

        public string PrimarySubject { get; set; }

        public List<string?>? UsersImagesUrl { get; set; }

        public int CommentsCount { get; set; }

        public DateTime CreatedTime { get; set; }

        public GetSubjectFilesDTO File { get; set; }
    }
}
