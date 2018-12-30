using System.Collections.Generic;
using Domain.Interfaces;
using MicroDocum.Themes.DefaultTheme.Attributes;
using Utils.WebRequests;

namespace DTO.Public.TODO
{
    [ServiceName("Web")]
    public class ListTODOResponse : Pagination<TodoPublicEntity>
    {
        public ListTODOResponse(): base()
        {
            //for mapping and deserialization
        }

        public ListTODOResponse(Pagination<TodoPublicEntity> pagination) : base(pagination)
        {
            
        }
        
        public ListTODOResponse(IEnumerable<TodoPublicEntity> items, int totalItems, IPaginationSetting settings) : base(items, totalItems, settings)
        {
        }
    }
}