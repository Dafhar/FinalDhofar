using DhofarAppApi.Dtos.SubjectType;

namespace DhofarAppApi.Dtos.Main
{
    public class SubjectForMainDTO
    {
        public required string Title { get; set; }

        public List<SubjectTypeDTO> subjectTypeDTOs { get; set; }
    }
}
