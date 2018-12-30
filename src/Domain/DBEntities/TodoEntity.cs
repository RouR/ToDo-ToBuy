using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Interfaces;

namespace Domain.DBEntities
{
    [Table("Todo")]
    public class TodoEntity : DBEntity, IForUser
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        
        [MaxLength(1000)]
        public string Description { get; set; }
        
        public virtual Guid UserId { get; set; }
    }
}
