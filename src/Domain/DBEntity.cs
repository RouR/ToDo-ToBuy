using System;
using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;

namespace Domain
{
    public abstract class DBEntity : ITimestampedEntity, IPublicIdEntity
    {
        [Key]
        public Guid PublicId { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsDeleted { get; set; }
    }
}