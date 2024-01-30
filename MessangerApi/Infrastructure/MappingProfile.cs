using AutoMapper;
using MessangerApi.Data.Dto;
using MessangerApi.Data.Model;

namespace MessangerApi.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Message, MessageDto>(MemberList.Destination).ReverseMap();
        }
    }

}
