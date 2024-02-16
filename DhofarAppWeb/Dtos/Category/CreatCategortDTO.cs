using DhofarAppWeb.Dtos.SubCategory;

namespace DhofarAppWeb.Dtos.Category
{
    public class CreatCategortDTO
    {

        public string Name_Ar { get; set; }
        public string Name_En { get; set; }

        public List<CreateSubCategoryDTO> subcategories { get; set; }
    }
}
