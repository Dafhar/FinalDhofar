using Microsoft.AspNetCore.Mvc;
using DhofarAppApi.Dtos.Category;
using DhofarAppWeb.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DhofarAppApi.InterFaces;
using DhofarAppApi.Dtos.SubCategory;
using DhofarAppApi.Model;

namespace DhofarAppApi.Controllers
{
    [ApiController]

    [Route("api/[controller]/[Action]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _categoryService;

        public CategoryController(ICategory categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/Category
        // ToDo: Tested 

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAll();
            return Ok(categories);
        }

        // GET: api/Category/5
        // ToDo: Tested 

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetById(id);

            if (category == null)
            {

                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);
            }

            return Ok(category);
        }

        // ToDo: Tested 

        [HttpGet("{categoryId}/Subcategories")]
        public async Task<ActionResult<IEnumerable<SubCategoryDTO>>> GetAllSubcategoriesByCategoryId(int categoryId)
        {
            var subcategories = await _categoryService.GetAllSubcategoriesByCategoryId(categoryId);
            if (subcategories == null || subcategories.Count == 0)
            {

                var errorResponse = new { errors = new { message = "Not found" } };
                return BadRequest(errorResponse);

            }

            return Ok(subcategories);
        }

        // ToDo: Tested 

        [HttpGet("{categoryId}/Subcategory/{subCategoryId}")]
        public async Task<ActionResult<SubCategoryDTO>> GetSubcategoryById(int categoryId, int subCategoryId)
        {
            var subCategoryDto = await _categoryService.GetSubcategoryById(categoryId, subCategoryId);

            if (subCategoryDto == null)
            {
                var errorResponse = new { errors = new { message = "Not found" } };
                    return BadRequest(errorResponse);
                
            }

            return subCategoryDto;
        }


    }
}
