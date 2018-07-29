using System;

namespace Domain.Interfaces
{
    public interface ITimestampedEntity
    {
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
    }
}
