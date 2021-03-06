﻿using System;
using Domain.DBEntities;
using Newtonsoft.Json;

namespace DTO.Public.TODO
{
    public class TodoPublicEntity : TodoEntity
    {
        [JsonIgnore]
        public override Guid UserId { get; set; }
        
        [JsonIgnore]
        public override bool IsDeleted { get; set; }
    }
}