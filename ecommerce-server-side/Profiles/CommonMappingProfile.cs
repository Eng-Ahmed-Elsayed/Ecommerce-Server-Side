using AutoMapper;
using Models.Models;

namespace Models.DataTransferObjects
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {
            CreateMap<CategoryDto, Category>();
            CreateMap<Category, CategoryDto>();

        }
    }
}
