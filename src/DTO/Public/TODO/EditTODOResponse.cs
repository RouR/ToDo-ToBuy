using DTO.Internal.TODO;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Public.TODO
{
    [ServiceName("Web")]
    public class EditTODOResponse : TodoPublicEntity, IProduceSometimes<UpdateTODO>
    {
    }
}