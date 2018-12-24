using Domain.DBEnities;
using DTO.Public.TOBUY;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Internal.TOBUY
{
    [ServiceName("TOBUYSrv")]
    public class CreateTOBUY : TobuyEntity, IProduce<SaveTOBUYResponse>
    {
    }
}
