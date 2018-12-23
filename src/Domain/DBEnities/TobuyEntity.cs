using System;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.DBEnities
{
    public class TobuyEntity : DBEntity, IForUser
    {
        public string Name { get; set; }
        public float Qty{ get; set; }
        public Price Price{ get; set; }
        public DateTime? DueToUtc{ get; set; }
        public virtual Guid UserId { get; set; }
    }
}
