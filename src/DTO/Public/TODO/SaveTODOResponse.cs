using Domain.DBEnities;
using Domain.Interfaces;
using MicroDocum.Themes.DefaultTheme.Attributes;

namespace DTO.Public.TODO
{
    [ServiceName("Web")]
    public class SaveTODOResponse : IErrorable<TodoPublicEntity>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public TodoPublicEntity Data { get; set; }
    }
}
