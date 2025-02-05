using AutoMapper;
using TagsApi.Dtos;
using TagsApi.Models;

namespace TagsApi.Mappers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Tag, TagDto>();
        }
    }
}
