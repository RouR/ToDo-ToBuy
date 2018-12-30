using System;
using Domain.Interfaces;
using DTO.Public.TOBUY;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Internal.TOBUY
{
    [ServiceName(nameof(Service.ToBuy))]
    public class DeleteTOBUY : IPublicIdEntity, IForUser, IProduce<DeleteTOBUYResponse>
    {
        public Guid PublicId { get; set; }
        public Guid UserId { get; set; }
    }
}