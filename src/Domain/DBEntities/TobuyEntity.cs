using System;
using System.ComponentModel.DataAnnotations;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.DBEntities
{
    public class TobuyEntity : DBEntity, IForUser
    {
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        public float Qty{ get; set; }
        public Price Price{ get; set; }
        public DateTime? DueToUtc{ get; set; }
        public virtual Guid UserId { get; set; }
    }
}
