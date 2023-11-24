using AutoMapper;
using Models.DataTransferObjects.Auth;
using Models.DataTransferObjects.Customer;
using Models.DataTransferObjects.Shared;
using Models.Models;


namespace ecommerce_server_side.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserForRegistrationDto, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.UserName));
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserAddress, UserAddressDto>().ReverseMap();
            CreateMap<UserPayment, UserPaymentDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartDto>().ReverseMap();
            CreateMap<CheckList, CheckListDto>().ReverseMap();
            CreateMap<CheckListItem, CheckListItemDto>().ReverseMap();
        }
    }
}
