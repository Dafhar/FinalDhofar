using DhofarAppApi.Dtos.Subject;

namespace DhofarAppApi.Dtos.GeneralSubject
{
    public class GetGeneralSubjectTypeToAllDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SubjectsCounter { get; set; }

        public int CommentsCounter { get; set; }

        public List<string> TopUsersImages { get; set; }

        //public List<GetSubjectDTO> Subjects { get; set; }
    }
}
