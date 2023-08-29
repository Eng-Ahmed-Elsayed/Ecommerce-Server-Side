using AutoMapper;

namespace Models.DataTransferObjects
{
    public class CommonMappingProfile : Profile
    {
        public CommonMappingProfile()
        {
            //CreateMap<UserForRegistrationDto, User>()
            //    .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
            //CreateMap<EmployeeDto, Employee>();
            //CreateMap<Employee, EmployeeDto>();
        }
    }
}
