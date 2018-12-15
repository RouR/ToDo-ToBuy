using System;
using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;

namespace Domain
{
    public abstract class DBEntity : ITimestampedEntity, IPublicIdEntity
    {
        [Key]
        public Guid PublicId { get; set; }

        public virtual DateTime Created { get; set; }
        public virtual DateTime Updated { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}