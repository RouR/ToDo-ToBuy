using System;
using Domain.Interfaces;
using DTO.Internal.TODO;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Public.TODO
{
    [ServiceName("Web")]
    public class DeleteTODORequest: IPublicIdEntity, IProduce<DeleteTODO>
    {
        public Guid PublicId { get; set; }
    }
}