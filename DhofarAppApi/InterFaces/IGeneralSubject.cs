using DhofarAppApi.Dtos.GeneralSubject;
using DhofarAppApi.Dtos.SubjectType;
using DhofarAppApi.Model;
using System.Security.Claims;

namespace DhofarAppApi.InterFaces
{
    public interface IGeneralSubject
    {
       public Task<List<GetGeneralSubjectTypeToAllDTO>> GetAllSubjectTypesForAll();

        public Task<List<GetGeneralSubjectTypeDTO>> GetAllSubjectTypesForAdmin();

        public Task<GetSinlgeGeneralSubjectTypeDTO> GetSubjectTypeById(int id);

       public Task<GeneralSubjectsType> CreateSubjectType(PostGeneralSubjectTypeDTO postSubjectTypeDto);

       public Task<GeneralSubjectsType> UpdateSubjectType(int id, PostGeneralSubjectTypeDTO postSubjectTypeDto);

       public Task<bool> DeleteSubjectType(int id);
    }
}
