using DhofarAppApi.Dtos.Subject;

namespace DhofarAppApi.Dtos.GeneralSubject
{
    public class GetSinlgeGeneralSubjectTypeDTO
    {
        public int Id { get; set; }

        public string Name  { get; set; }

        public List<ListOfAllSubjectByGeneralSubjectTypeDTO> Subjects { get; set; }
    }
}
