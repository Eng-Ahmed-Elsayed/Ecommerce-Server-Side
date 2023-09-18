using AutoMapper;
using Models.Models;

namespace Models.DataTransferObjects
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
            //CreateMap<Category, CategoryDto>();

        }
    }
}
