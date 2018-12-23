using Domain.DBEnities;
using Domain.Interfaces;
using DTO.Internal.TOBUY;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Public.TOBUY
{
    [ServiceName("Web")]
    public class ListTOBUYRequest : IPaginationSetting, ISortable, IProduce<ListTOBUY>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public bool Asc { get; set; }
    }
}