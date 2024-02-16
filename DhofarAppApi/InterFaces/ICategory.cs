using DhofarAppApi.Dtos.Category;
using DhofarAppApi.Dtos.Complaint;
using DhofarAppApi.Dtos.SubCategory;
using DhofarAppApi.Model;
using System.Security.Claims;

namespace DhofarAppApi.InterFaces
{
    public interface ICategory
    {
        Task<List<CategoryDTO>> GetAll();
        Task<CategoryDTO> GetById(int id);
       
        Task<List<SubCategoryDTO>> GetAllSubcategoriesByCategoryId(int categoryId);
       
        Task<SubCategoryDTO> GetSubcategoryById(int categoryId, int subCategoryId);



    }
}
