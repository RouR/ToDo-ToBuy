using System;
using Domain.DBEnities;
using Domain.Models;
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