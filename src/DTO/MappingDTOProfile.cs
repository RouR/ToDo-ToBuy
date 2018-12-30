using AutoMapper;
using Domain.DBEntities;
using DTO.Internal.Account;
using DTO.Internal.TOBUY;
using DTO.Internal.TODO;
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
            CreateMap<ListTOBUYRequest, ListTOBUY>();
            CreateMap<SaveTOBUYRequest, UpdateTOBUY>();
            CreateMap<SaveTOBUYRequest, CreateTOBUY>();
            CreateMap<DeleteTOBUYRequest, DeleteTOBUY>();
            CreateMap<ListTODORequest, ListTODO>();
            CreateMap<SaveTODORequest, UpdateTODO>();
            CreateMap<SaveTODORequest, CreateTODO>();
            CreateMap<DeleteTODORequest, DeleteTODO>();
        }
    }
}