using System;
using DTO.Internal.TOBUY;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Public.TOBUY
{
    [ServiceName("Web")]
    public class SaveTOBUYRequest : IProduceSometimes<CreateTOBUY>, IProduceSometimes<UpdateTOBUY>
    {
        public Guid? PublicId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
