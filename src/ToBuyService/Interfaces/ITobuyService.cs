using DTO.Internal.TOBUY;
using DTO.Public.TOBUY;

namespace ToBuyService.Interfaces
{
    public interface ITobuyService
    {
        ListTOBUYResponse List(ListTOBUY request);
        FindToBuyResponse Find(FindToBuyRequest request);
        SaveTOBUYResponse Create(CreateTOBUY request);
        SaveTOBUYResponse Update(UpdateTOBUY request);
        DeleteTOBUYResponse Delete(DeleteTOBUY request);
    }
}