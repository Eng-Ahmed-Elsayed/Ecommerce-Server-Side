using AutoMapper;
using Models.DataTransferObjects.Customer;
using Models.DataTransferObjects.Shared;
using Models.Models;

namespace Models.DataTransferObjects
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {

            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<DiscountDto, Discount>().ReverseMap();
            CreateMap<Inventory, InventoryDto>().ReverseMap();
            CreateMap<ShippingOption, ShippingOptionDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();

            CreateMap<OrderDetails, OrderDetailsDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemDto>().ReverseMap();

            CreateMap<OrderItem, CartItem>(MemberList.Source)
                .ForMember(dest => dest.Id, act => act.Ignore())
                .ForMember(dest => dest.CreatedAt, act => act.Ignore())
                .ForMember(dest => dest.UpdatedAt, act => act.Ignore())
                .ForMember(dest => dest.DeletedAt, act => act.Ignore())
                .ReverseMap()
                ;

            CreateMap<OrderItemDto, CartItemDto>(MemberList.Source)
               .ForMember(dest => dest.Id, act => act.Ignore())
               .ReverseMap()
               ;

            CreateMap<Review, ReviewDto>().ReverseMap();


        }
    }
}
