using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;
using MicroDocum.Themes.DefaultTheme.Attributes;
using Utils.WebRequests;

namespace DTO.Public.TODO
{
    [ServiceName("Web")]
    public class ListTODOResponse : Pagination<TodoPublicEntity>
    {
        public ListTODOResponse(IEnumerable<TodoPublicEntity> items, int totalItems, IPaginationSetting settings) : base(items, totalItems, settings)
        {
        }
    }
}