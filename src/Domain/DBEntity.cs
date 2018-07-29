using System;
using System.Runtime.Serialization;
using Domain.Interfaces;

namespace Domain
{
    public abstract class DBEntity : ITimestampedEntity, IPublicIdEntity
    {
        [IgnoreDataMember]
        long Id { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public Guid PublicId { get; set; }
    }
}