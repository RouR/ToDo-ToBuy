using System;
using Domain.Interfaces;
using DTO.Public.TODO;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Internal.TODO
{
    [ServiceName(nameof(Service.ToDo))]
    public class DeleteTODO : IPublicIdEntity, IForUser, IProduce<DeleteTODOResponse>
    {
        public Guid PublicId { get; set; }
        public Guid UserId { get; set; }
    }
}