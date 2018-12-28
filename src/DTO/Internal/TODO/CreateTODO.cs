using Domain.DBEntities;
using DTO.Public.TODO;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Internal.TODO
{
    [ServiceName(nameof(Service.ToDo))]
    public class CreateTODO : TodoEntity, IProduce<SaveTODOResponse>
    {
    }
}
