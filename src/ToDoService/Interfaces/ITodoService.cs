using DTO.Internal.TODO;
using DTO.Public.TODO;

namespace ToDoService.Interfaces
{
    public interface ITodoService
    {
        ListTODOResponse List(ListTODO request);
        FindToDoResponse Find(FindToDoRequest request);
        SaveTODOResponse Create(CreateTODO request);
        SaveTODOResponse Update(UpdateTODO request);
        DeleteTODOResponse Delete(DeleteTODO request);
    }
}