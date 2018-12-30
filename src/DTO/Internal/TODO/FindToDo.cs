using System;
using Domain.DBEntities;
using Domain.Interfaces;
using DTO.Public.TODO;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Internal.TODO
{
    [ServiceName(nameof(Service.ToBuy))]
    public class FindToDoRequest: IForUser, IProduce<FindToDoResponse>
    {
        public Guid UserId { get; set; }
        public Guid PublicId { get; set; }
    }
    
    [ServiceName(nameof(Service.ToBuy))]
    public class FindToDoResponse: IErrorable<TodoEntity>, IProduce<EditTODOResponse>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public TodoEntity Data { get; set; }
    }
}