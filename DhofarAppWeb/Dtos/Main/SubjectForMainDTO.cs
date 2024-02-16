using DhofarAppWeb.Dtos.SubjectType;

namespace DhofarAppWeb.Dtos.Main
{
    public class SubjectForMainDTO
    {
        public required string Title { get; set; }

        public List<SubjectTypeDTO> subjectTypeDTOs { get; set; }
    }
}
