using AutoMapper;
using UserAPI.Data.Dto;
using UserAPI.Data.Model;

namespace UserAPI.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>(MemberList.Destination).ReverseMap();
        }
    }
}
