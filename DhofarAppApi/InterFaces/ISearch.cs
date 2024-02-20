using DhofarAppApi.Dtos.Search;

namespace DhofarAppApi.InterFaces
{
    public interface ISearch
    {
        Task<List<SubjectBySearchNameDTO>> SearchForSubject(string subjectName);
    }
}
