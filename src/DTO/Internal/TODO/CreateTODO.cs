using Domain.DBEnities;
using DTO.Public.TODO;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Internal.TODO
{
    [ServiceName("ToDoSrv")]
    public class CreateTODO : TodoEntity, IProduce<SaveTODOResponse>
    {
    }
}
