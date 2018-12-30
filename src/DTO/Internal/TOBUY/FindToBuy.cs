using System;
using Domain.DBEntities;
using Domain.Interfaces;
using DTO.Public.TOBUY;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Internal.TOBUY
{
    [ServiceName(nameof(Service.ToBuy))]
    public class FindToBuyRequest: IForUser, IProduce<FindToBuyResponse>
    {
        public Guid UserId { get; set; }
        public Guid PublicId { get; set; }
    }
    
    [ServiceName(nameof(Service.ToBuy))]
    public class FindToBuyResponse: IErrorable<TobuyEntity>, IProduce<EditTOBUYResponse>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public TobuyEntity Data { get; set; }
    }
}