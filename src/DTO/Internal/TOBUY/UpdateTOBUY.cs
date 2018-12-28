using Domain.DBEntities;
using DTO.Public.TOBUY;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Internal.TOBUY
{
    [ServiceName(nameof(Service.ToBuy))]
    public class UpdateTOBUY: TobuyEntity, IProduce<SaveTOBUYResponse>
    {
    }
}