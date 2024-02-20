using DhofarAppApi.Dtos.Subject;

namespace DhofarAppApi.Dtos.GeneralSubject
{
    public class GetGeneralSubjectTypeDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }

        public int SubjectsCounter { get; set; }

        public int CommentsCounter { get; set; }

        public List<string> TopUsersImages { get; set; }

    }
}
