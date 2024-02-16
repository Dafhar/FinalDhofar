using DhofarAppApi.Dtos.DepartmentType;
using DhofarAppApi.Model;
using System.Security.Claims;

namespace DhofarAppApi.InterFaces
{
    public interface IDepartmentType
    {
        Task<DepartmentTypeDTO> CreateDepartmentType(DepartmentTypeDTO departmentType);
        Task<List<GetDepartmentType>> GetAllDepartmentTypes();
        Task<GetDepartmentType> GetDepartmentTypeById(int id);
        Task<DepartmentTypeDTO> UpdateDepartmentType(int id, DepartmentTypeDTO updatedDepartmentType);
        Task<bool> DeleteDepartmentType(int id);
    }
}
