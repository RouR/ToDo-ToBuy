using AutoMapper;
using Domain.DBEntities;
using DTO.Internal.Account;
using DTO.Public.Account;
using DTO.Public.TOBUY;
using DTO.Public.TODO;

namespace DTO
{
    public class MappingDTOProfile : Profile
    {
        public MappingDTOProfile()
        {
            CreateMap<TodoEntity, TodoPublicEntity>();
            CreateMap<RegisterRequest, CreateUserRequest>();
            CreateMap<TodoPublicEntity, TodoEntity>();
            CreateMap<TOBUYPublicEntity, TobuyEntity>();
        }
    }
}