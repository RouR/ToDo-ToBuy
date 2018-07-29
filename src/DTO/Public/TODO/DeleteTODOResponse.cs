using MicroDocum.Themes.DefaultTheme.Attributes;

namespace DTO.Public.TODO
{
    [ServiceName("Web")]
    public class DeleteTODOResponse : IErrorable<bool>
    {
        public bool HasError { get; set; }
        public string Message { get; set; }
        public bool Data { get; set; }
    }
}