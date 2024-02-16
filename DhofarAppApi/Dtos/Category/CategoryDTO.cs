using DhofarAppApi.Dtos.SubCategory;
using DhofarAppApi.Model;

namespace DhofarAppApi.Dtos.Category
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<SubCategoryDTO> subcategories { get; set; }

    }
}
