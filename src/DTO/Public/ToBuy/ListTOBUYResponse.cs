using System.Collections.Generic;
using Domain.Interfaces;
using MicroDocum.Themes.DefaultTheme.Attributes;
using Utils.WebRequests;

namespace DTO.Public.TOBUY
{
    [ServiceName("Web")]
    public class ListTOBUYResponse : Pagination<TOBUYPublicEntity>
    {
        public ListTOBUYResponse(): base()
        {
            //for mapping and deserialization
        }
        public ListTOBUYResponse(Pagination<TOBUYPublicEntity> @from) : base(@from)
        {
        }

        public ListTOBUYResponse(IEnumerable<TOBUYPublicEntity> items, int totalItems, IPaginationSetting settings) : base(items, totalItems, settings)
        {
        }
    }
}