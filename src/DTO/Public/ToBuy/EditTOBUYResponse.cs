using Domain.Interfaces;
using MicroDocum.Themes.DefaultTheme.Attributes;

namespace DTO.Public.TOBUY
{
    [ServiceName("Web")]
    public class EditTOBUYResponse : IErrorable<TOBUYPublicEntity>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public TOBUYPublicEntity Data { get; set; }
    }
}