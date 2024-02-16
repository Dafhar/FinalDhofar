using DhofarAppApi.Dtos.SubjectType;
using DhofarAppApi.Model;
using System.Security.Claims;

namespace DhofarAppApi.InterFaces
{
    public interface ISubjectType
    {
       public Task<List<GetSubjectTypeDTO>> GetAllSubjectTypes();
       public Task<GetSubjectTypeDTO> GetSubjectTypeById(int id);
       public Task<SubjectType> CreateSubjectType(PostSubjectTypeDTO postSubjectTypeDto);
       public Task<SubjectType> UpdateSubjectType(int id, PostSubjectTypeDTO postSubjectTypeDto);
       public Task<bool> DeleteSubjectType(int id);
    }
}
