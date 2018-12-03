using System;
using Domain.DBEnities;
using Newtonsoft.Json;

namespace DTO.Public.TODO
{
    public class TodoPublicEntity : TodoEntity
    {
        [JsonIgnore]
        public override Guid UserId { get; set; }
    }
}