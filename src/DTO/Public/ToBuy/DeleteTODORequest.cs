using System;
using Domain.Interfaces;
using DTO.Internal.TOBUY;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Public.TOBUY
{
    [ServiceName("Web")]
    public class DeleteTOBUYRequest: IPublicIdEntity, IProduce<DeleteTOBUY>
    {
        public Guid PublicId { get; set; }
    }
}