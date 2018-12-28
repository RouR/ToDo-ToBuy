using Domain.DBEntities;
using Domain.Interfaces;
using DTO.Internal.TODO;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Public.TODO
{
    [ServiceName("Web")]
    public class ListTODORequest : IPaginationSetting, ISortable, IFilterable<TODOFilter, TodoEntity>, IProduce<ListTODO>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }
        public bool Asc { get; set; }
        public TODOFilter Filter { get; set; } = new TODOFilter();
    }
}