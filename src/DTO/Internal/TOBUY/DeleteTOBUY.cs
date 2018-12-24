using System;
using Domain.Interfaces;
using DTO.Public.TOBUY;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Internal.TOBUY
{
    [ServiceName("TOBUYSrv")]
    public class DeleteTOBUY : IPublicIdEntity, IProduce<DeleteTOBUYResponse>
    {
        public Guid PublicId { get; set; }
    }
}