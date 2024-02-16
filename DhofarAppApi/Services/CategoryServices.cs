using DhofarAppApi.Data;
using DhofarAppApi.Dtos.Category;
using DhofarAppApi.Dtos.SubCategory;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace DhofarAppApi.Services
{
    public class CategoryServices : ICategory
    {
        private readonly AppDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWTTokenServices _jWTTokenServices;

        public CategoryServices(AppDbContext db, IHttpContextAccessor httpContextAccessor, JWTTokenServices jWTTokenServices)
        {
            _db = db;
            _httpContextAccessor= httpContextAccessor;
            _jWTTokenServices=jWTTokenServices;
        }

       


        public async Task<List<CategoryDTO>> GetAll()
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenServices.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _db.Users.FindAsync(userId); var allCategories = await _db.Categories
                .Include(c => c.subcategories)
                .ToListAsync();

            var categoryDtos = allCategories.Select(category =>
            {
                return new CategoryDTO
                {
                    Id = category.Id,
                    Name = user.SelectedLanguage == "en" ? category.Name_En : category.Name_Ar,
                    subcategories = category.subcategories.Select(subcategory => new SubCategoryDTO
                    {
                        Id =subcategory.Id,
                        CategoryId = subcategory.CategoryId,
                        Name = user.SelectedLanguage == "en" ? subcategory.Name_En : subcategory.Name_Ar,
                    }).ToList()
                };
            }).ToList();

            return categoryDtos;
        }

        public async Task<CategoryDTO> GetById(int id)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenServices.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _db.Users.FindAsync(userId);
            var category = await _db.Categories
                .Include(c => c.subcategories)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return null;
            }

            var categoryDto = new CategoryDTO
            {
                Id = category.Id,
                Name = user.SelectedLanguage == "en" ? category.Name_En : category.Name_Ar,

                subcategories = category.subcategories.Select(subcategory => new SubCategoryDTO
                {
                    Id = subcategory.Id,
                    CategoryId = subcategory.CategoryId,
                    Name = user.SelectedLanguage == "en" ? category.Name_En : category.Name_Ar,
                }).ToList()
            };

            return categoryDto;
        }

        public async Task<List<SubCategoryDTO>> GetAllSubcategoriesByCategoryId(int categoryId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenServices.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _db.Users.FindAsync(userId); var subcategories = await _db.SubCategories
                .Where(s => s.CategoryId == categoryId)
                .Select(s => new SubCategoryDTO
                {
                    Id = s.Id,
                    CategoryId = s.CategoryId,
                    Name = user.SelectedLanguage == "en" ? s.Name_En : s.Name_Ar,
                })
                .ToListAsync();

            return subcategories;
        }

        public async Task<SubCategoryDTO> GetSubcategoryById(int categoryId, int subCategoryId)
        {
            var JwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var decodedJwt = _jWTTokenServices.DecodeJwt(JwtToken);
            var userId = decodedJwt.Claims.FirstOrDefault(c => c.Type == "nameid").Value;
            var user = await _db.Users.FindAsync(userId);
            var subCategory = await _db.SubCategories
                .FirstOrDefaultAsync(s => s.CategoryId == categoryId && s.Id == subCategoryId);

            if (subCategory == null)
            {
                return null; // Subcategory not found
            }


            var subCategoryDto = new SubCategoryDTO
            {
                Id = subCategory.Id,
                CategoryId = subCategory.CategoryId,
                Name = user.SelectedLanguage == "en" ? subCategory.Name_En : subCategory.Name_Ar,
            };

            return subCategoryDto;
        }


       


       
    }
}
