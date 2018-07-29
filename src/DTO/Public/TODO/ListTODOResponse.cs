using System.Collections.Generic;
using Domain.DBEnities;
using MicroDocum.Themes.DefaultTheme.Attributes;
using Utils.Pagination;

namespace DTO.Public.TODO
{
    [ServiceName("Web")]
    public class ListTODOResponse : Pagination<TodoEntity>
    {
        public ListTODOResponse(IEnumerable<TodoEntity> items, int totalItems, IPaginationSetting settings) : base(items, totalItems, settings)
        {
        }
    }
}