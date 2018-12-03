using AutoMapper;
using Domain.DBEnities;
using DTO.Public.TODO;

namespace DTO
{
    public class MappingDTOProfile : Profile
    {
        public MappingDTOProfile()
        {
            CreateMap<TodoEntity, TodoPublicEntity>();
        }
    }
}