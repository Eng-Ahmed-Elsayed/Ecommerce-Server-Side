using AutoMapper;
using Models.DataTransferObjects.Shared;
using Models.Models;

namespace Models.DataTransferObjects
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {

            CreateMap<Category, CategoryDto>()
                .ReverseMap();

            CreateMap<Product, ProductDto>().ReverseMap(); ;
            CreateMap<ProductDto, Product>();
            CreateMap<DiscountDto, Discount>().ReverseMap();
            CreateMap<Inventory, InventoryDto>().ReverseMap();


        }
    }
}
