using DhofarAppWeb.Dtos.SubCategory;
using DhofarAppWeb.Model;

namespace DhofarAppWeb.Dtos.Category
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<SubCategoryDTO> subcategories { get; set; }

    }
}
