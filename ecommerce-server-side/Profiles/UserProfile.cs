using AutoMapper;
using Models.DataTransferObjects.Auth;
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
        }
    }
}
