using DTO.Internal.TODO;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;
using Utils.Pagination;

namespace DTO.Public.TODO
{
    [ServiceName("Web")]
    public class ListTODORequest : IPaginationSetting, IProduce<ListTODO>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
