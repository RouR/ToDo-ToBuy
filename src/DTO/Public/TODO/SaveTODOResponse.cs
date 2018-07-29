using Domain.DBEnities;
using MicroDocum.Themes.DefaultTheme.Attributes;

namespace DTO.Public.TODO
{
    [ServiceName("Web")]
    public class SaveTODOResponse : IErrorable<TodoEntity>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public TodoEntity Data { get; set; }
    }
}
