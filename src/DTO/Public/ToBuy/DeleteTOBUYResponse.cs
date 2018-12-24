using Domain.Interfaces;
using MicroDocum.Themes.DefaultTheme.Attributes;

namespace DTO.Public.TOBUY
{
    [ServiceName("Web")]
    public class DeleteTOBUYResponse : IErrorable<bool>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public bool Data { get; set; }
    }
}