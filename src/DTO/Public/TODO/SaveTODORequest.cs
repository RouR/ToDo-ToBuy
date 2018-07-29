using DTO.Internal.TODO;
using MicroDocum.Themes.DefaultTheme.Attributes;
using MicroDocum.Themes.DefaultTheme.Interfaces;

namespace DTO.Public.TODO
{
    [ServiceName("Web")]
    public class SaveTODORequest : IProduceSometimes<CreateTODO>, IProduceSometimes<UpdateTODO>
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
