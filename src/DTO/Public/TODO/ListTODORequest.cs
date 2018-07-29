using MicroDocum.Themes.DefaultTheme.Attributes;
using Utils.Pagination;

namespace DTO.Public.TODO
{
    [ServiceName("Web")]
    public class ListTODORequest : IPaginationSetting
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
