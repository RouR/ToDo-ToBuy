using System;

namespace Domain.Interfaces
{
    public interface IForUser
    {
        Guid UserId { get; set; }
    }
}
