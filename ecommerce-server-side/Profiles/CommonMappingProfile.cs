using AutoMapper;
using Models.Models;

namespace Models.DataTransferObjects
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<DiscountDto, Discount>().ReverseMap();

        }
    }
}
