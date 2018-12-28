using System;
using Domain.DBEntities;
using Newtonsoft.Json;

namespace DTO.Public.TOBUY
{
    public class TOBUYPublicEntity : TobuyEntity
    {
        [JsonIgnore]
        public override Guid UserId { get; set; }
        
        [JsonIgnore]
        public override bool IsDeleted { get; set; }
    }
}