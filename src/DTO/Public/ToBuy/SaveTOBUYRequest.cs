using System;
using Domain.Models;
using DTO.Internal.TOBUY;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Public.TOBUY
{
    [ServiceName("Web")]
    public class SaveTOBUYRequest : IProduceSometimes<CreateTOBUY>, IProduceSometimes<UpdateTOBUY>
    {
        public Guid? PublicId { get; set; }
        public string Name { get; set; }
        public float Qty{ get; set; }
        public Price Price{ get; set; }
        public DateTime? DueToUtc{ get; set; }
    }
}
