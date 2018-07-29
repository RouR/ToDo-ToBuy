using System;
using Domain.Interfaces;

namespace Domain.DBEnities
{
    public class TodoEntity : DBEntity, IUser
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
    }
}
