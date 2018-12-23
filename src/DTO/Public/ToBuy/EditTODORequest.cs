using System;
using DTO.Internal.TOBUY;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Public.TOBUY
{
    [ServiceName("Web")]
    public class EditTOBUYRequest : IProduceSometimes<UpdateTOBUY>
    {
        public Guid? PublicId { get; set; }
    }
}