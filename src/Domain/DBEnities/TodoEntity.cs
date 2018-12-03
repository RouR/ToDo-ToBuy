using System;
using Domain.Interfaces;

namespace Domain.DBEnities
{
    public class TodoEntity : DBEntity, IForUser
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public virtual Guid UserId { get; set; }
    }
}
