using ECommerce.Core.DTO.Category;

namespace ECommerce.Api.Mapping
{
    public class CategoryMapping : Profile
    {
        public CategoryMapping()
        {
            CreateMap<CategoryDTO, Category>()
                .ReverseMap();
        }
    }
}
