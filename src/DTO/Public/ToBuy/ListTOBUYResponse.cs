using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;
using MicroDocum.Themes.DefaultTheme.Attributes;
using Utils.WebRequests;

namespace DTO.Public.TOBUY
{
    [ServiceName("Web")]
    public class ListTOBUYResponse : Pagination<TOBUYPublicEntity>
    {
        public ListTOBUYResponse(IEnumerable<TOBUYPublicEntity> items, int totalItems, IPaginationSetting settings) : base(items, totalItems, settings)
        {
        }
    }
}